using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CatalogService.Api.Controllers;
using CatalogService.Application.Queries.CategoryQueries;
using CatalogService.Application.Commands.CategoryCommands;
using CatalogService.Application.Dtos;

namespace CatalogService.Unit.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CategoriesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfDtos()
    {
        var dtos = new List<CategoryDto> { new CategoryDto { Id = 1, Name = "Name1" } };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(dtos);

        var result = await _controller.GetAll();

        result.Should().BeEquivalentTo(dtos);
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsOkObject()
    {
        var dto = new CategoryDto { Id = 1, Name = "Name1" };
        _mediatorMock.Setup(m => m.Send(It.Is<GetCategoryByIdQuery>(q => q.Id == 1), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(dto);

        var actionResult = await _controller.GetById(1);
        var okResult = actionResult.Result as OkObjectResult;

        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((CategoryDto?)null);

        var actionResult = await _controller.GetById(999);

        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var command = new AddCategoryCommand { Name = "New" };
        var dto = new CategoryDto { Id = 5, Name = "New" };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(dto);

        var actionResult = await _controller.Create(command);
        var created = actionResult as CreatedAtActionResult;

        created.Should().NotBeNull();
        (created!.Value as CategoryDto)!.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Update_IdMismatch_ReturnsBadRequest()
    {
        var cmd = new UpdateCategoryCommand { Id = 2, Name = "X" };
        var result = await _controller.Update(1, cmd);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Update_WhenSuccess_ReturnsNoContent()
    {
        var cmd = new UpdateCategoryCommand { Id = 3, Name = "X" };
        _mediatorMock.Setup(m => m.Send(cmd, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        var result = await _controller.Update(3, cmd);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_WhenNotFound_ReturnsNotFound()
    {
        var cmd = new UpdateCategoryCommand { Id = 4, Name = "X" };
        _mediatorMock.Setup(m => m.Send(cmd, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);

        var result = await _controller.Update(4, cmd);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_WhenSuccess_ReturnsNoContent()
    {
        _mediatorMock.Setup(m => m.Send(It.Is<DeleteCategoryCommand>(c => c.Id == 1), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);

        var result = await _controller.Delete(9);

        result.Should().BeOfType<NotFoundResult>();
    }
}
