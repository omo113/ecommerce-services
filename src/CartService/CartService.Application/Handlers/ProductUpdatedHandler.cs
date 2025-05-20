using CartService.Domain.Handlers;
using CartService.Domain.Handlers.Events;
using CartService.Domain.Repositories;
using EcommerceServices.Shared.ValueObjects;
using System.Linq;
using System.Threading;

namespace CartService.Application.Handlers;

public class ProductUpdatedHandler : IProductUpdatedHandler
{
    private readonly ICartRepository _cartRepository;

    public ProductUpdatedHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task Handle(ProductUpdatedEvent updatedEvent)
    {
        var carts = await _cartRepository.GetAll(CancellationToken.None);
        foreach (var cart in carts)
        {
            var itemsToUpdate = cart.Items
                .Where(i => i.ItemId == updatedEvent.UId.ToString())
                .ToList();
            if (!itemsToUpdate.Any()) continue;

            foreach (var item in itemsToUpdate)
            {
                item.Name = updatedEvent.Name;
                item.Price = Money.Create(updatedEvent.Price.Amount, updatedEvent.Price.Currency);
            }

            await _cartRepository.UpdateCartAsync(cart, CancellationToken.None);
        }
    }
}