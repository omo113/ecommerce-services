using CatalogService.Domain.Repositories;
using MediatR;

namespace CatalogService.Application.Commands.CategoryCommands;

public record DeleteCategoryCommand(int Id) : IRequest<bool>;
public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryToDelete = await categoryRepository.GetByIdAsync(request.Id);

        if (categoryToDelete == null)
        {
            return false;
        }

        await categoryRepository.DeleteAsync(categoryToDelete);
        return true;
    }
}