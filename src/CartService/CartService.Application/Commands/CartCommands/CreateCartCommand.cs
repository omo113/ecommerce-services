using CartService.Application.Dtos;
using CartService.Domain.Entities.CartEntity;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using EcommerceServices.Shared.ValueObjects;
using FluentValidation;
using MediatR;
using OneOf;

namespace CartService.Application.Commands.CartCommands;


public record CreateCartCommand(string Id, string Name, ImageDto? Image, MoneyDto Price, int Quantity) : IRequest<OneOf<bool, Error>>;

public class CreateCartCommandValidation : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Image)
            .NotNull();
        RuleFor(x => x.Price)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Price.Amount)
                    .NotNull()
                    .GreaterThan(0);
                RuleFor(x => x.Price.Currency)
                    .NotNull()
                    .IsInEnum();
            });
        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}

public class CreateCartCommandHandler(ICartRepository cartRepository) : IRequestHandler<CreateCartCommand, OneOf<bool, Error>>
{
    public async Task<OneOf<bool, Error>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        if (await cartRepository.IdExists(request.Id, cancellationToken))
            return new Error("CartAlreadyExists", "Cart with this ID already exists");

        var existing = await cartRepository.GetCartById(request.Id, cancellationToken);
        var item = new CartItem
        {
            ItemId = Guid.NewGuid().ToString(),
            Name = request.Name,
            Image = request.Image != null ? new Image { Url = request.Image.Url, Alt = request.Image.Alt } : null,
            Price = new Money { Amount = request.Price.Amount, Currency = request.Price.Currency },
            Quantity = request.Quantity
        };
        if (existing is null)
        {
            var cart = new Cart
            {
                Id = request.Id,
                CreatedAt = TimeProvider.System.GetUtcNow()
            };
            cart.AddItem(item);
            await cartRepository.CreateCartAsync(cart, cancellationToken);
        }
        else
        {
            existing.AddItem(item);
            await cartRepository.UpdateCartAsync(existing, cancellationToken);
        }
        return true;
    }
}