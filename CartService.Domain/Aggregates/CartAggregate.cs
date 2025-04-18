namespace CartService.Domain.Aggregates;

public class CartAggregate
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Image? Image { get; set; }
    public Money Price { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class Image
{
    public string Url { get; set; }
    public string Alt { get; set; }
}

public class Money
{
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
}
public enum Currency
{
    USD,
    EUR,
    GBP,
    JPY,
    AUD,
    CAD,
    CHF,
    CNY,
    SEK,
    NZD
}