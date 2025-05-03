using CatalogService.Application.Dtos;
using CatalogService.Domain.Repositories;
using EcommerceServices.Shared;
using MediatR;

namespace CatalogService.Application.Queries.ProductQueries;

public record GetAllProductsQueryModel(string? Category, int? Id) : Query;

public record GetAllProductsQuery(GetAllProductsQueryModel Query) : IRequest<IEnumerable<ProductDto>>;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await productRepository.ListAsync(product =>
                (
                    request.Query.Category == null ||
                    product.Category.Name.ToLower().Contains(request.Query.Category))
                &&
                (request.Query.Id == null && product.Id == request.Query.Id)
            );
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}