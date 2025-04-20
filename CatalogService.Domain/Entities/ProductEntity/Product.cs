using CatalogService.Domain.Entities.CategoryEntity;
using CatalogService.Domain.ValueObjects;
using EcommerceServices.Shared;

namespace CatalogService.Domain.Entities.ProductEntity;

public class Product : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Amount { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public Image? Image { get; private set; }

    private Product(string name, string description, decimal price, int amount, int categoryId, Image? image)
    {
        Name = name;
        Description = description;
        Price = price;
        Amount = amount;
        CategoryId = categoryId;
        Image = image;
        CreateDate = TimeProvider.System.GetUtcNow();
    }
    public static Product Create(string name, string description, decimal price, int amount, int categoryId, Image? image)
    {
        return new Product(name, description, price, amount, categoryId, image);
    }
}