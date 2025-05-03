using CatalogService.Application.Commands.CategoryCommands;
using CatalogService.Application.Commands.ProductCommands;
using CatalogService.Application.Dtos;
using CatalogService.Integration.Tests.Abstractions;
using EcommerceServices.Shared;
using EcommerceServices.Shared.Hateoas;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace CatalogService.Integration.Tests;

public class ProductTests(CatalogWebAppFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetAllProducts_ReturnsOkAndList()
    {
        // Act
        var response = await Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/products"));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetProductById_InvalidId_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync("api/products/9999");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedWithDto()
    {
        // Arrange: create category
        var catCmd = new AddCategoryCommand { Name = "ProdCat" };
        var catResp = await Client.PostAsJsonAsync("api/categories", catCmd);
        catResp.EnsureSuccessStatusCode();
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>()!;

        var command = new AddProductCommand
        {
            Name = "TestProduct",
            Description = "Desc",
            Image = null,
            CategoryId = cat!.Id,
            Price = new MoneyDto(9.99m, Currency.USD),
            Amount = 10
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/products", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<ProductDto>()!;
        dto.ShouldNotBeNull();
        dto.Name.ShouldBe("TestProduct");
        dto.CategoryId.ShouldBe(cat.Id);
    }

    [Fact]
    public async Task GetProductById_ValidId_ReturnsOkAndResourceWithLinks()
    {
        // Arrange: create category and product
        var catCmd = new AddCategoryCommand { Name = "ProdCat2" };
        var catResp = await Client.PostAsJsonAsync("api/categories", catCmd);
        catResp.EnsureSuccessStatusCode();
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>()!;
        var prodCmd = new AddProductCommand
        {
            Name = "Prod2",
            Description = null,
            Image = null,
            CategoryId = cat!.Id,
            Price = new MoneyDto(5m, Currency.USD),
            Amount = 1
        };
        var prodResp = await Client.PostAsJsonAsync("api/products", prodCmd);
        prodResp.EnsureSuccessStatusCode();
        var prod = await prodResp.Content.ReadFromJsonAsync<ProductDto>()!;

        // Act
        var response = await Client.GetAsync($"api/products/{prod!.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var resource = await response.Content.ReadFromJsonAsync<Resource<ProductDto>>()!;
        resource!.Data.Id.ShouldBe(prod!.Id);
        resource.Data.Name.ShouldBe("Prod2");
        resource.Links.ShouldContain(l => l.Rel == "self");
        resource.Links.ShouldContain(l => l.Rel == "update");
        resource.Links.ShouldContain(l => l.Rel == "delete");
        resource.Links.ShouldContain(l => l.Rel == "all");
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContentAndPersistsChange()
    {
        // Arrange: create category and product
        var catCmd = new AddCategoryCommand { Name = "ProdCat3" };
        var catResp = await Client.PostAsJsonAsync("api/categories", catCmd);
        catResp.EnsureSuccessStatusCode();
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>()!;
        var prodCmd = new AddProductCommand
        {
            Name = "Prod3",
            Description = null,
            Image = null,
            CategoryId = cat!.Id,
            Price = new MoneyDto(7m, Currency.USD),
            Amount = 2
        };
        var prodResp = await Client.PostAsJsonAsync("api/products", prodCmd);
        prodResp.EnsureSuccessStatusCode();
        var prod = await prodResp.Content.ReadFromJsonAsync<ProductDto>()!;

        var updateCmd = new UpdateProductCommand
        {
            Id = prod!.Id,
            Name = "Prod3Updated",
            Description = prod.Description,
            Image = prod.Image,
            CategoryId = prod.CategoryId,
            Price = prod.Price,
            Amount = prod.Amount
        };

        // Act
        var putResp = await Client.PutAsJsonAsync($"api/products/{prod!.Id}", updateCmd);

        // Assert
        putResp.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        var getResp = await Client.GetAsync($"api/products/{prod!.Id}");
        var resource = await getResp.Content.ReadFromJsonAsync<Resource<ProductDto>>()!;
        resource!.Data.Name.ShouldBe("Prod3Updated");
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContentThenNotFound()
    {
        // Arrange: create category and product
        var catCmd = new AddCategoryCommand { Name = "ProdCat4" };
        var catResp = await Client.PostAsJsonAsync("api/categories", catCmd);
        catResp.EnsureSuccessStatusCode();
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>()!;
        var prodCmd = new AddProductCommand
        {
            Name = "Prod4",
            Description = null,
            Image = null,
            CategoryId = cat!.Id,
            Price = new MoneyDto(8m, Currency.USD),
            Amount = 3
        };
        var prodResp = await Client.PostAsJsonAsync("api/products", prodCmd);
        prodResp.EnsureSuccessStatusCode();
        var prod = await prodResp.Content.ReadFromJsonAsync<ProductDto>()!;

        // Act
        var delResp = await Client.DeleteAsync($"api/products/{prod!.Id}");

        // Assert
        delResp.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        var getResp = await Client.GetAsync($"api/products/{prod!.Id}");
        getResp.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

}