using CartService.Application.Dtos;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using MediatR;
using OneOf;

namespace CartService.Application.Queries.CartQueries;

public record CartsQuery : IRequest<OneOf<List<CartDto>, Error>>;


public class CartsQueryHandler(ICartRepository cartRepository) : IRequestHandler<CartsQuery, OneOf<List<CartDto>, Error>>
{
    public async Task<OneOf<List<CartDto>, Error>> Handle(CartsQuery request, CancellationToken cancellationToken)
    {
        var res = await cartRepository.GetAll(cancellationToken);
        return res.ConvertAll(x => new CartDto
        {
            Id = x.Id,
            Image = x.Image != null ? new ImageDto(x.Image.Url, x.Image.Alt) : null,
            Name = x.Name,
            Price = new MoneyDto(x.Price.Amount, x.Price.Currency),
            Quantity = x.Quantity
        });
    }
}