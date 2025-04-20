using CatalogService.Application.Dtos;
using CatalogService.Domain.Repositories;
using MediatR;

namespace CatalogService.Application.Queries.ProductQueries;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var p = await _productRepository.GetByIdAsync(request.Id);
        if (p == null) return null;
        return new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image?.Url,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name,
            Price = new MoneyDto(p.Price.Amount, p.Price.Currency),
            Amount = p.Amount
        };
    }
}