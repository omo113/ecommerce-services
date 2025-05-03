using CartService.Application.Commands.CartCommands;
using CartService.Application.Dtos;
using CartService.Domain.Entities.CartEntity;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using FluentAssertions;
using Moq;

namespace CartService.Unit.Tests
{
    public class CreateCartCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnTrueAndCallCreate_WhenIdDoesNotExist()
        {
            // Arrange
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock.Setup(r => r.IdExists("cart1", It.IsAny<CancellationToken>()))
                          .ReturnsAsync(false);
            var handler = new CreateCartCommandHandler(repositoryMock.Object);
            var command = new CreateCartCommand(
                 "cart1",
                 "cart1",
                 null,
                new MoneyDto(100m, Currency.USD),
                 2);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsT0.Should().BeTrue();
            repositoryMock.Verify(r => r.CreateCartAsync(
                It.Is<Cart>(c => c.Name == "cart1" &&
                                 c.Price.Amount == 100m &&
                                 c.Price.Currency == Currency.USD &&
                                 c.Quantity == 2),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenIdAlreadyExists()
        {
            // Arrange
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock.Setup(r => r.IdExists("cart1", It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);
            var handler = new CreateCartCommandHandler(repositoryMock.Object);
            var command = new CreateCartCommand(
                "cart1",
                "cart1",
                null,
                new MoneyDto(50m, Currency.EUR),
                1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsT1.Should().BeTrue();
            var error = result.AsT1;
            error.ErrorCode.Should().Be("CartAlreadyExists");
            error.Description.Should().Contain("Cart with this ID already exists");
            repositoryMock.Verify(r => r.CreateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}