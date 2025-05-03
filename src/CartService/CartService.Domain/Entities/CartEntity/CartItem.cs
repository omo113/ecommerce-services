using EcommerceServices.Shared.ValueObjects;

namespace CartService.Domain.Entities.CartEntity;

public class CartItem
{
    public string ItemId { get; set; }
    public string Name { get; set; }
    public Image? Image { get; set; }
    public Money Price { get; set; }
    public int Quantity { get; set; }
}