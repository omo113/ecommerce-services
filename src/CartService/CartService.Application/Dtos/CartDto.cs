namespace CartService.Application.Dtos;

public class CartDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required MoneyDto Price { get; set; }
    public required ImageDto? Image { get; set; }
    public required int Quantity { get; set; }
}