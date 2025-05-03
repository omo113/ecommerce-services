using EcommerceServices.Shared.ValueObjects;

namespace CartService.Domain.Entities.CartEntity;

public class Cart
{
    public string Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<CartItem> Items { get; set; } = new();

    public void AddItem(CartItem item)
    {
        Items.Add(item);
    }

    public bool RemoveItem(string itemId)
    {
        var item = Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item == null) return false;
        Items.Remove(item);
        return true;
    }
}