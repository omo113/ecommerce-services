using CatalogService.Application.Dtos;
using CatalogService.Domain.Entities.CategoryEntity;
using CatalogService.Domain.Repositories;
using EcommerceServices.Shared.ValueObjects;
using FluentValidation;
using MediatR;

namespace CatalogService.Application.Commands.CategoryCommands;


public class AddCategoryCommand : IRequest<CategoryDto> // Returns the newly created category DTO
{
    public string Name { get; set; }
    public ImageDto? Image { get; set; }
    public int? ParentCategoryId { get; set; }
}
public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
    }
}
public class AddCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<AddCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.ParentCategoryId.HasValue)
        {
            var parentExists = await categoryRepository.GetByIdAsync(request.ParentCategoryId.Value);
            if (parentExists == null)
            {
                throw new Exception($"Parent category with ID {request.ParentCategoryId.Value} not found.");
            }
        }

        var image = request.Image != null ? Image.Create(request.Image.Url, request.Image.Alt) : null;
        var category = Category.Create(request.Name, image, request.ParentCategoryId);

        await categoryRepository.AddAsync(category);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Image = request.Image,
            ParentCategoryId = category.ParentCategoryId,
        };
    }
}