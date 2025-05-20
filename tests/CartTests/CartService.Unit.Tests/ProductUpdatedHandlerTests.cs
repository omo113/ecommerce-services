using CartService.Application.Handlers;
using CartService.Domain.Entities.CartEntity;
using CartService.Domain.Handlers.Events;
using CartService.Domain.Repositories;
using EcommerceServices.Shared;
using EcommerceServices.Shared.ValueObjects;
using Moq;

namespace CartService.Unit.Tests
{
    public class ProductUpdatedHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUpdateCartItems_WhenMatchingItemsExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var priceDto = new MoneyDto { Amount = 123.45m, Currency = Currency.USD };
            var updatedEvent = new ProductUpdatedEvent
            {
                UId = productId,
                Name = "NewName",
                Description = "Desc",
                Price = priceDto,
                Amount = 10
            };

            var cart = new Cart
            {
                Id = "cart1",
                Items = new List<CartItem>
                {
                    new() { ItemId = productId.ToString(), Name = "OldName", Price = Money.Create(50m, Currency.EUR), Quantity = 2 },
                    new() { ItemId = "other", Name = "Other", Price = Money.Create(100m, Currency.USD), Quantity = 1 }
                }
            };

            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new List<Cart> { cart });
            repositoryMock.Setup(r => r.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            var handler = new ProductUpdatedHandler(repositoryMock.Object);

            // Act
            await handler.Handle(updatedEvent);

            // Assert
            repositoryMock.Verify(r => r.UpdateCartAsync(
                It.Is<Cart>(c =>
                    c.Items.First(i => i.ItemId == productId.ToString()).Name == "NewName" &&
                    c.Items.First(i => i.ItemId == productId.ToString()).Price.Amount == 123.45m &&
                    c.Items.First(i => i.ItemId == productId.ToString()).Price.Currency == Currency.USD
                ),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotCallUpdate_WhenNoMatchingItems()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updatedEvent = new ProductUpdatedEvent
            {
                UId = productId,
                Name = "Name",
                Description = "Desc",
                Price = new MoneyDto { Amount = 1m, Currency = Currency.USD },
                Amount = 1
            };

            var cart = new Cart
            {
                Id = "cart1",
                Items = new List<CartItem>
                {
                    new() { ItemId = "other", Name = "Other", Price = Money.Create(100m, Currency.USD), Quantity = 1 }
                }
            };

            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new List<Cart> { cart });

            var handler = new ProductUpdatedHandler(repositoryMock.Object);

            // Act
            await handler.Handle(updatedEvent);

            // Assert
            repositoryMock.Verify(r => r.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
