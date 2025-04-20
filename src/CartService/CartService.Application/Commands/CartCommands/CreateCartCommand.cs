using CartService.Application.Dtos;
using CartService.Domain.Entities.CartEntity;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using EcommerceServices.Shared.ValueObjects;
using FluentValidation;
using MediatR;
using OneOf;

namespace CartService.Application.Commands.CartCommands;


public record CreateCartCommand(string Name, ImageDto? Image, MoneyDto Price, int Quantity) : IRequest<OneOf<bool, Error>>;

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
        if (await cartRepository.IdExists(request.Name, cancellationToken))
        {
            return new Error("CartAlreadyExists", "Cart with this ID already exists");
        }

        var cart = new Cart
        {
            CreatedAt = TimeProvider.System.GetUtcNow(),
            Name = request.Name,
            Price = new Money
            {
                Amount = request.Price.Amount,
                Currency = request.Price.Currency,
            },
            Quantity = request.Quantity
        };
        if (request.Image != null)
        {
            cart.Image = new Image
            {
                Url = request.Image.Url,
                Alt = request.Image.Alt
            };
        }
        await cartRepository.CreateCartAsync(cart, cancellationToken);
        return true;
    }
}