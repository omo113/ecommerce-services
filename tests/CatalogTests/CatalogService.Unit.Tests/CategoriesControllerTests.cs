using CatalogService.Api.Controllers;
using CatalogService.Application.Commands.CategoryCommands;
using CatalogService.Application.Dtos;
using CatalogService.Application.Queries.CategoryQueries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatalogService.Unit.Tests;

public class CategoriesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        // Arrange: Create mock mediator and instantiate the controller
        _mediatorMock = new Mock<IMediator>();
        _controller = new CategoriesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfDtos()
    {
        // Arrange
        var dtos = new List<CategoryDto> { new() { Id = 1, Name = "Name1" } };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(dtos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeEquivalentTo(dtos);
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsOkObject()
    {
        // Arrange
        var dto = new CategoryDto { Id = 1, Name = "Name1" };
        _mediatorMock.Setup(m => m.Send(It.Is<GetCategoryByIdQuery>(q => q.Id == 1), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(dto);

        // Act
        var actionResult = await _controller.GetById(1);
        var okResult = actionResult.Result as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((CategoryDto?)null);

        // Act
        var actionResult = await _controller.GetById(999);

        // Assert
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        // Arrange
        var command = new AddCategoryCommand { Name = "New" };
        var dto = new CategoryDto { Id = 5, Name = "New" };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(dto);

        // Act
        var actionResult = await _controller.Create(command);
        var created = actionResult.Result as CreatedAtActionResult;

        // Assert
        created.Should().NotBeNull();
        (created!.Value as CategoryDto)!.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Update_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var cmd = new UpdateCategoryCommand { Id = 2, Name = "X" };

        // Act
        var result = await _controller.Update(1, cmd);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Update_WhenSuccess_ReturnsNoContent()
    {
        // Arrange
        var cmd = new UpdateCategoryCommand { Id = 3, Name = "X" };
        _mediatorMock.Setup(m => m.Send(cmd, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        // Act
        var result = await _controller.Update(3, cmd);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var cmd = new UpdateCategoryCommand { Id = 4, Name = "X" };
        _mediatorMock.Setup(m => m.Send(cmd, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);

        // Act
        var result = await _controller.Update(4, cmd);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_WhenSuccess_ReturnsNoContent()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.Is<DeleteCategoryCommand>(c => c.Id == 1), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(9);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
