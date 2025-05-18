using CatalogService.Application.Dtos;
using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Kafka;
using EcommerceServices.Shared.ValueObjects;
using FluentValidation;
using MediatR;

namespace CatalogService.Application.Commands.CategoryCommands;

public record UpdateCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ImageDto? Image { get; set; }
    public int? ParentCategoryId { get; set; }
}
public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name.Length)
            .LessThanOrEqualTo(50);
    }
}
public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, KafkaEventPublisher kafkaEventPublisher)
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