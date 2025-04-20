namespace CatalogService.Application.Dtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ImageDto? Image { get; set; }
    public int? ParentCategoryId { get; set; }
}