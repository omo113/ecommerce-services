using CatalogService.Domain.Entities.CategoryEntity;
using CatalogService.Domain.Entities.ProductEntity;
using CatalogService.Infrastructure.Persistance;
using CatalogService.Infrastructure.Repositories;
using EcommerceServices.Shared;
using EcommerceServices.Shared.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Unit.Tests;

public class RepositoryIntegrationTests
{
    private CatalogDbContext CreateContext(string name)
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(name)
            .Options;
        return new CatalogDbContext(options);
    }

    [Fact]
    public async Task ProductRepository_ShouldAddAndListProduct()
    {
        // Arrange
        var context = CreateContext("Db1");
        var catRepo = new CategoryRepository(context);
        var prodRepo = new ProductRepository(context);
        var category = Category.Create("C1", null, null);
        await catRepo.AddAsync(category);
        var product = Product.Create("P1", "D", Money.Create(1, Currency.AUD), 2, category.Id, null);

        // Act
        await prodRepo.AddAsync(product);
        var list = await prodRepo.ListAllAsync();

        // Assert
        list.Should().ContainSingle(p => p.Name == "P1" && p.CategoryId == category.Id);
    }
}