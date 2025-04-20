using CatalogService.Application.Commands.CategoryCommands;
using CatalogService.Application.Dtos;
using CatalogService.Integration.Tests.Abstractions;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace CatalogService.Integration.Tests;

public class CatalogTest(CatalogWebAppFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetAllCategories_ReturnsOkAndList()
    {
        // Arrange

        // Act
        var response = await Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/categories"));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        categories.ShouldNotBeNull();
        categories.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetCategoryById_InvalidId_ReturnsNotFound()
    {
        // Arrange

        // Act
        var response = await Client.GetAsync("api/categories/9999");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreatedWithDto()
    {
        // Arrange
        var command = new AddCategoryCommand { Name = "TestCategory" };

        // Act
        var response = await Client.PostAsJsonAsync("api/categories", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<CategoryDto>();
        dto.ShouldNotBeNull();
        dto!.Name.ShouldBe("TestCategory");
    }
}
