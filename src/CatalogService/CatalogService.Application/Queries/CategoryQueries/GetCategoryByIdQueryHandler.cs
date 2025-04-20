using CatalogService.Application.Dtos;
using CatalogService.Domain.Repositories;
using MediatR;

namespace CatalogService.Application.Queries.CategoryQueries;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto?>;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id);

        if (category == null)
        {
            return null;
        }

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Image = category.Image != null ? new ImageDto(category.Image.Url, category.Image.Alt) : null,
            ParentCategoryId = category.ParentCategoryId
        };
    }
}