namespace CatalogService.Domain.ValueObjects;

public class Money
{
    public int Id { get; private set; }
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }

    public static Money Create(decimal amount, Currency currency)
    {
        return new Money
        {
            Amount = amount,
            Currency = currency
        };
    }
}