using CatalogService.Domain.ValueObjects;
using EcommerceServices.Shared;

namespace CatalogService.Domain.Entities.CategoryEntity;

public class Category : Entity
{
    public string Name { get; private set; }
    public Image? Image { get; private set; }
    public int ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    private Category(string name, Image? image, int parentCategoryId)
    {
        Name = name;
        Image = image;
        ParentCategoryId = parentCategoryId;
        CreateDate = TimeProvider.System.GetUtcNow();
    }

    public static Category Create(string name, Image? image, int parentCategoryId)
    {
        return new Category(name, image, parentCategoryId);
    }
}