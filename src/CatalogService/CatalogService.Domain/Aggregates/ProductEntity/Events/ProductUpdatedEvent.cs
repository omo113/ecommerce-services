using EcommerceServices.Shared;

namespace CatalogService.Domain.Aggregates.ProductEntity.Events;

public class ProductUpdatedEvent : DomainEvent
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public MoneyDto Price { get; private set; }
    public int Amount { get; private set; }
}

public class MoneyDto
{
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
}