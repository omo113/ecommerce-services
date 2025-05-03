using CartService.Application.Dtos;
using CartService.Application.Errors;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using MediatR;
using OneOf;
using System.Linq;

namespace CartService.Application.Queries.CartQueries;

public record GetCartItemsQuery(string CartId) : IRequest<OneOf<List<CartItemDto>, Error>>;

public class GetCartItemsQueryHandler(ICartRepository cartRepository)
    : IRequestHandler<GetCartItemsQuery, OneOf<List<CartItemDto>, Error>>
{
    public async Task<OneOf<List<CartItemDto>, Error>> Handle(
        GetCartItemsQuery request,
        CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetCartById(request.CartId, cancellationToken);
        if (cart is null)
            return CartErrors.CartNotExist;
        var items = cart.Items.Select(item => new CartItemDto
        {
            CartId = cart.Id,
            ItemId = item.ItemId,
            Name = item.Name,
            Image = item.Image != null ? new ImageDto(item.Image.Url, item.Image.Alt) : null,
            Price = new MoneyDto(item.Price.Amount, item.Price.Currency),
            Quantity = item.Quantity
        }).ToList();
        return items;
    }
}