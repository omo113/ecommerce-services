using CartService.Domain.Handlers.Events;

namespace CartService.Domain.Handlers;

public interface IProductUpdatedHandler
{
    Task Handle(ProductUpdatedEvent updatedEvent);
}

