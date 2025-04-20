using CatalogService.Application.Dtos;
using CatalogService.Domain.Repositories;
using EcommerceServices.Shared.ValueObjects;
using FluentValidation;
using MediatR;

namespace CatalogService.Application.Commands.ProductCommands;

public record UpdateProductCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int CategoryId { get; set; }
    public MoneyDto Price { get; set; }
    public int Amount { get; set; }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Image)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .When(x => !string.IsNullOrEmpty(x.Image));
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.Price.Amount)
            .GreaterThan(0);
        RuleFor(x => x.Price.Currency)
            .IsInEnum();
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
    }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null) return false;

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null) throw new Exception($"Category with ID {request.CategoryId} not found.");

        var imageVo = !string.IsNullOrEmpty(request.Image)
            ? Image.Create(request.Image, string.Empty)
            : null;

        product.Update(request.Name, request.Description ?? string.Empty, Money.Create(request.Price.Amount, request.Price.Currency), request.Amount, request.CategoryId, imageVo);
        await _productRepository.UpdateAsync(product);

        return true;
    }
}