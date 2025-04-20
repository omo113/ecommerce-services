namespace EcommerceServices.Shared.ValueObjects;

public class Image : ValueObject
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Alt { get; set; }

    public static Image Create(string url, string alt) => new()
    {
        Url = url,
        Alt = alt
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
        yield return Alt;
    }
}