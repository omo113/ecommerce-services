using CatalogService.Domain.Aggregates.CategoryEntity;
using CatalogService.Domain.Aggregates.ProductEntity.Events;
using EcommerceServices.Shared;
using EcommerceServices.Shared.ValueObjects;

namespace CatalogService.Domain.Aggregates.ProductEntity;

public class Product : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public int Amount { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public Image? Image { get; private set; }

    private Product()
    {

    }
    private Product(string name, string description, Money price, int amount, int categoryId, Image? image)
    {
        Name = name;
        Description = description;
        Amount = amount;
        CategoryId = categoryId;
        Image = image;
        Price = price;
        CreateDate = TimeProvider.System.GetUtcNow();
    }

    public static Product Create(string name, string description, Money price, int amount, int categoryId, Image? image)
    {
        return new Product(name, description, price, amount, categoryId, image);
    }

    public void Update(string name, string description, Money price, int amount, int categoryId, Image? image)
    {
        Name = name;
        Description = description;
        Price = price;
        Amount = amount;
        CategoryId = categoryId;
        Image = image;
        LastChangeDate = TimeProvider.System.GetUtcNow();
        Raise(new ProductUpdatedEvent
        {
            Price = new MoneyDto
            {
                Amount = price.Amount,
                Currency = price.Currency
            },
            Amount = amount,
            Description = description,
            Name = name
        });
    }
}