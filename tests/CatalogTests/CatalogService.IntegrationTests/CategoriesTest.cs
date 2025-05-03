//using CatalogService.Application.Commands.CategoryCommands;
//using CatalogService.Application.Dtos;
//using CatalogService.Integration.Tests.Abstractions;
//using EcommerceServices.Shared.Hateoas;
//using Shouldly;
//using System.Net;
//using System.Net.Http.Json;

//namespace CatalogService.Integration.Tests;

//public class CatalogTest(CatalogWebAppFactory factory) : IntegrationTestBase(factory)
//{
//    [Fact]
//    public async Task GetAllCategories_ReturnsOkAndList()
//    {
//        // Arrange

//        // Act
//        var response = await Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/categories"));

//        // Assert
//        response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
//    }

//    [Fact]
//    public async Task GetCategoryById_InvalidId_ReturnsNotFound()
//    {
//        // Arrange

//        // Act
//        var response = await Client.GetAsync("api/categories/9999");

//        // Assert
//        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
//    }

//    [Fact]
//    public async Task CreateCategory_ReturnsCreatedWithDto()
//    {
//        // Arrange
//        var command = new AddCategoryCommand { Name = "TestCategory" };

//        // Act
//        var response = await Client.PostAsJsonAsync("api/categories", command);

//        // Assert
//        response.StatusCode.ShouldBe(HttpStatusCode.Created);
//        var dto = await response.Content.ReadFromJsonAsync<CategoryDto>()!;
//        dto.ShouldNotBeNull();
//        dto!.Name.ShouldBe("TestCategory");
//    }

//    [Fact]
//    public async Task GetCategoryById_ValidId_ReturnsOkAndResourceWithLinks()
//    {
//        // Arrange: create a category
//        var create = new AddCategoryCommand { Name = "IntegrationCat" };
//        var createResp = await Client.PostAsJsonAsync("api/categories", create);
//        createResp.EnsureSuccessStatusCode();
//        var dto = await createResp.Content.ReadFromJsonAsync<CategoryDto>()!;

//        // Act: retrieve by id
//        var resp = await Client.GetAsync($"api/categories/{dto.Id}");

//        // Assert
//        resp.StatusCode.ShouldBe(HttpStatusCode.OK);
//        var resource = await resp.Content.ReadFromJsonAsync<Resource<CategoryDto>>()!;
//        resource.ShouldNotBeNull();
//        resource.Data.Id.ShouldBe(dto.Id);
//        resource.Data.Name.ShouldBe("IntegrationCat");
//        resource.Links.ShouldContain(l => l.Rel == "self");
//        resource.Links.ShouldContain(l => l.Rel == "update");
//        resource.Links.ShouldContain(l => l.Rel == "delete");
//        resource.Links.ShouldContain(l => l.Rel == "all");

//    }

//    [Fact]
//    public async Task UpdateCategory_ReturnsNoContentAndPersistsChange()
//    {
//        // Arrange: create
//        var create = new AddCategoryCommand { Name = "OldName" };
//        var createResp = await Client.PostAsJsonAsync("api/categories", create);
//        var dto = await createResp.Content.ReadFromJsonAsync<CategoryDto>()!;

//        // Act: update
//        var updateCmd = new UpdateCategoryCommand { Id = dto.Id, Name = "NewName" };
//        var putResp = await Client.PutAsJsonAsync($"api/categories/{dto.Id}", updateCmd);

//        // Assert
//        putResp.StatusCode.ShouldBe(HttpStatusCode.NoContent);
//        var getResp = await Client.GetAsync($"api/categories/{dto.Id}");
//        var resource = await getResp.Content.ReadFromJsonAsync<Resource<CategoryDto>>()!;
//        resource.Data.Name.ShouldBe("NewName");
//    }

//    [Fact]
//    public async Task DeleteCategory_ReturnsNoContentThenNotFound()
//    {
//        // Arrange: create
//        var create = new AddCategoryCommand { Name = "ToDelete" };
//        var createResp = await Client.PostAsJsonAsync("api/categories", create);
//        var dto = await createResp.Content.ReadFromJsonAsync<CategoryDto>()!;

//        // Act: delete
//        var delResp = await Client.DeleteAsync($"api/categories/{dto.Id}");

//        // Assert
//        delResp.StatusCode.ShouldBe(HttpStatusCode.NoContent);
//        var getResp = await Client.GetAsync($"api/categories/{dto.Id}");
//        getResp.StatusCode.ShouldBe(HttpStatusCode.NotFound);
//    }

//}
