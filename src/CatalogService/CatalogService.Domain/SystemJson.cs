using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatalogService.Domain;

public class SystemJson
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };
}
