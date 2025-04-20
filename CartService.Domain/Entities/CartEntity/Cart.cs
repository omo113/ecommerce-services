using CartService.Domain.Entities.CartEntity.ValueObjects;

namespace CartService.Domain.Entities.CartEntity;

public class Cart
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Image? Image { get; set; }
    public Money Price { get; set; }
    public int Quantity { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}