namespace CartService.Application.Dtos;

public class CartItemDto
{
    public required string CartId { get; set; }
    public required string ItemId { get; set; }
    public required string Name { get; set; }
    public required ImageDto? Image { get; set; }
    public required MoneyDto Price { get; set; }
    public required int Quantity { get; set; }
}