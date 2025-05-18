using EcommerceServices.Shared;

namespace CatalogService.Domain.Aggregates.ProductEntity.Events;

public class ProductUpdatedEvent : DomainEvent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public MoneyDto Price { get; set; }
    public int Amount { get; set; }
}

public class MoneyDto
{
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
}