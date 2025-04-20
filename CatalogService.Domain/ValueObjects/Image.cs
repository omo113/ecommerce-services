namespace CatalogService.Domain.ValueObjects;

public class Image
{
    public int Id { get; private set; }
    public string Url { get; set; }
    public string Alt { get; set; }

    public static Image Create(string url, string alt) => new()
    {
        Url = url,
        Alt = alt
    };
}