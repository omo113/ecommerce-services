using CartService.Domain.Handlers;
using CartService.Domain.Handlers.Events;

namespace CartService.Infrastructure.Kafka;

public class ProductUpdatedHandler : IProductUpdatedHandler
{
    public Task Handle(ProductUpdatedEvent updatedEvent)
    {
        throw new NotImplementedException();
    }
}