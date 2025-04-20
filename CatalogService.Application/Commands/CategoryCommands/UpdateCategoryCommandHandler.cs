using CatalogService.Application.Dtos;
using CatalogService.Domain.Repositories;
using EcommerceServices.Shared.ValueObjects;
using FluentValidation;
using MediatR;

namespace CatalogService.Application.Commands.CategoryCommands;

public record UpdateCategoryCommand : IRequest<bool> // Returns true if successful, false otherwise (or void/Unit)
{
    public int Id { get; set; } // ID of the category to update
    public string Name { get; set; }
    public ImageDto? Image { get; set; }
    public int? ParentCategoryId { get; set; }
    // Note: Be careful about allowing ParentCategoryId changes to avoid circular references or orphaning.
}
public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
    }
}
public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<UpdateCategoryCommand, bool>
{
    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryToUpdate = await categoryRepository.GetByIdAsync(request.Id);

        var image = categoryToUpdate!.Image;
        if (request.Image != null)
        {
            var newImage = Image.Create(request.Image.Url, request.Image.Alt);
            image = newImage.Equals(image!) ? newImage : image;
        }
        categoryToUpdate.Update(request.Name, image, request.ParentCategoryId);

        await categoryRepository.UpdateAsync(categoryToUpdate);

        return true;
    }
}