using CatalogService.Application.Commands.ProductCommands;
using CatalogService.Application.Dtos;
using CatalogService.Domain.Entities.CategoryEntity;
using CatalogService.Domain.Entities.ProductEntity;
using CatalogService.Domain.Repositories;
using EcommerceServices.Shared;
using FluentAssertions;
using Moq;

namespace CatalogService.Unit.Tests;

public class AddProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnDto_WhenValidRequest()
    {
        // Arrange
        var mockProdRepo = new Mock<IProductRepository>();
        var mockCatRepo = new Mock<ICategoryRepository>();
        mockCatRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Category.Create("Cat1", null, null));
        var handler = new AddProductCommandHandler(mockProdRepo.Object, mockCatRepo.Object);
        var cmd = new AddProductCommand { Name = "P1", CategoryId = 1, Price = new MoneyDto(1, Currency.AUD), Amount = 5, Description = "Desc" };

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(cmd.Name);
        result.Price.Should().Be(cmd.Price);
        mockProdRepo.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCategoryNotFound()
    {
        // Arrange
        var mockProdRepo = new Mock<IProductRepository>();
        var mockCatRepo = new Mock<ICategoryRepository>();
        mockCatRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category?)null);
        var handler = new AddProductCommandHandler(mockProdRepo.Object, mockCatRepo.Object);
        var cmd = new AddProductCommand { Name = "P1", CategoryId = 99, Price = new MoneyDto(1, Currency.AUD), Amount = 5 };

        // Act
        Func<Task> act = async () => await handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Category with ID 99 not found.*");
    }
}