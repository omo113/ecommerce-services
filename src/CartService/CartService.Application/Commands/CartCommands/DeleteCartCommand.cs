using CartService.Application.Errors;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using MediatR;
using OneOf;

namespace CartService.Application.Commands.CartCommands;

public record DeleteCartCommand(string Id) : IRequest<OneOf<bool, Error>>;

public class DeleteCartCommandValidator(ICartRepository cartRepository)
    : IRequestHandler<DeleteCartCommand, OneOf<bool, Error>>
{

    public async Task<OneOf<bool, Error>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetCartById(request.Id, cancellationToken);
        if (cart is null)
            return CartErrors.CartNotExist;
        var res = await cartRepository.DeleteCartItem(cart, cancellationToken);
        return res ? res : new Error("CartAggregate", "Failed To Delete");
    }
}