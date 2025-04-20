using CatalogService.Application.Dtos;
using CatalogService.Domain.Repositories;
using MediatR;

namespace CatalogService.Application.Queries.CategoryQueries;
public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
public class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.ListAllAsync();

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Image = c.Image != null ? new ImageDto(c.Image.Url, c.Image.Alt) : null,
            ParentCategoryId = c.ParentCategoryId
        });
    }
}