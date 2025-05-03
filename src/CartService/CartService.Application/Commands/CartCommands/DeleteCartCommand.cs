using CartService.Application.Errors;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using MediatR;
using OneOf;

namespace CartService.Application.Commands.CartCommands;

public record DeleteCartCommand(string CartId, string ItemId) : IRequest<OneOf<bool, Error>>;

public class DeleteCartCommandHandler(ICartRepository cartRepository)
    : IRequestHandler<DeleteCartCommand, OneOf<bool, Error>>
{
    public async Task<OneOf<bool, Error>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetCartById(request.CartId, cancellationToken);
        if (cart is null)
            return CartErrors.CartNotExist;
        if (!cart.RemoveItem(request.ItemId))
            return new Error("CartAggregate", "Item not found in cart");
        await cartRepository.UpdateCartAsync(cart, cancellationToken);
        return true;
    }
}