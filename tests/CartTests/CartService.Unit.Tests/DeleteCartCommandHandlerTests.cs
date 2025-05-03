using CartService.Application.Commands.CartCommands;
using CartService.Application.Errors;
using CartService.Domain.Entities.CartEntity;
using CartService.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CartService.Unit.Tests
{
    public class DeleteCartCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnError_WhenCartDoesNotExist()
        {
            // Arrange
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock.Setup(r => r.GetCartById("id1", It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Cart?)null);
            var handler = new DeleteCartCommandHandler(repositoryMock.Object);
            var command = new DeleteCartCommand("id1", "item1");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsT1.Should().BeTrue();
            var error = result.AsT1;
            error.Should().BeEquivalentTo(CartErrors.CartNotExist);
            repositoryMock.Verify(r => r.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            // Arrange
            var existingCart = new Cart { Id = "id2", Items = new List<CartItem> { new CartItem { ItemId = "item1" } } };
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock.Setup(r => r.GetCartById("id2", It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingCart);
            repositoryMock.Setup(r => r.UpdateCartAsync(existingCart, It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);
            var handler = new DeleteCartCommandHandler(repositoryMock.Object);
            var command = new DeleteCartCommand("id2", "item1");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsT0.Should().BeTrue();
            repositoryMock.Verify(r => r.UpdateCartAsync(existingCart, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenDeleteFails()
        {
            // Arrange
            var existingCart = new Cart { Id = "id3", Items = new List<CartItem>() };
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock.Setup(r => r.GetCartById("id3", It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingCart);
            var handler = new DeleteCartCommandHandler(repositoryMock.Object);
            var command = new DeleteCartCommand("id3", "item1");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsT1.Should().BeTrue();
            var error = result.AsT1;
            error.ErrorCode.Should().Be("CartAggregate");
            error.Description.Should().Contain("Item not found in cart");
            repositoryMock.Verify(r => r.UpdateCartAsync(existingCart, It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}