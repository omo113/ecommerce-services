using CatalogService.Domain.ValueObjects;

namespace CartService.Domain.Entities.CartEntity.ValueObjects;

public class Money
{
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
}