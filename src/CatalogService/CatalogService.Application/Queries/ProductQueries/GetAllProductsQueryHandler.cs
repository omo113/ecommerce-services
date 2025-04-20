using CatalogService.Application.Dtos;
using CatalogService.Domain.Repositories;
using MediatR;

namespace CatalogService.Application.Queries.ProductQueries;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.ListAllAsync();
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image?.Url,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name,
            Price = new MoneyDto(p.Price.Amount, p.Price.Currency),
            Amount = p.Amount
        });
    }
}