namespace EcommerceServices.Shared.ValueObjects;

public class Money : ValueObject
{
    public int Id { get; private set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }

    public static Money Create(decimal amount, Currency currency)
    {
        return new Money
        {
            Amount = amount,
            Currency = currency
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}