using CatalogService.Application.Dtos;
using CatalogService.Domain.Entities.ProductEntity;
using CatalogService.Domain.Repositories;
using EcommerceServices.Shared.ValueObjects;
using FluentValidation;
using MediatR;

namespace CatalogService.Application.Commands.ProductCommands;

public class AddProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int CategoryId { get; set; }
    public MoneyDto Price { get; set; }
    public int Amount { get; set; }
}

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Image)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .When(x => !string.IsNullOrEmpty(x.Image));
    }
}

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public AddProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new Exception($"Category with ID {request.CategoryId} not found.");

        var image = !string.IsNullOrEmpty(request.Image)
            ? Image.Create(request.Image, string.Empty)
            : null;

        var product = Product.Create(
            request.Name,
            request.Description ?? string.Empty,
            Money.Create(request.Price.Amount, request.Price.Currency),
            request.Amount,
            request.CategoryId,
            image);

        await _productRepository.AddAsync(product);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Image = product.Image?.Url,
            CategoryId = product.CategoryId,
            CategoryName = category.Name,
            Price = request.Price,
            Amount = product.Amount
        };
    }
}