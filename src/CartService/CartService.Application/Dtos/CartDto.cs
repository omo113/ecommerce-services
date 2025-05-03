namespace CartService.Application.Dtos;

public class CartDto
{
    public required string Id { get; set; }
    public required List<CartItemDto> Items { get; set; }
}