using CartService.Application.Dtos;
using CartService.Application.Errors;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using MediatR;
using OneOf;
using System.Linq;

namespace CartService.Application.Queries.CartQueries;

public record GetCartQuery(string Id) : IRequest<OneOf<CartDto, Error>>;

public class GetCartQueryHandler(ICartRepository cartRepository)
    : IRequestHandler<GetCartQuery, OneOf<CartDto, Error>>
{
    public async Task<OneOf<CartDto, Error>> Handle(
        GetCartQuery request,
        CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetCartById(request.Id, cancellationToken);
        if (cart is null)
            return CartErrors.CartNotExist;
        return new CartDto
        {
            Id = cart.Id,
            Items = cart.Items.Select(item => new CartItemDto
            {
                CartId = cart.Id,
                ItemId = item.ItemId,
                Name = item.Name,
                Image = item.Image != null ? new ImageDto(item.Image.Url, item.Image.Alt) : null,
                Price = new MoneyDto(item.Price.Amount, item.Price.Currency),
                Quantity = item.Quantity
            }).ToList()
        };
    }
}