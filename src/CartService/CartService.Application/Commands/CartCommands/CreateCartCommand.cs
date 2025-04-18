using CartService.Application.Dtos;
using EcommerceServices.Shared;
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
            .NotEmpty()
            .Must(x =>
            {
                Console.WriteLine();
                return true;
            });
        RuleFor(x => x.Image)
            .NotNull();
        RuleFor(x => x.Price)
            .NotNull();
        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, OneOf<bool, Error>>
{
    public Task<OneOf<bool, Error>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(OneOf<bool, Error>.FromT0(true));
    }
}