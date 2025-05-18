using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatalogService.Infrastructure.Persistance.Configurations;

internal static class ValueConversionExtensions
{
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
    {
        var settings = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };

        ValueConverter<T, string> converter = new(
            v => JsonSerializer.Serialize(v, settings),
            v => (JsonSerializer.Deserialize<T>(v, settings) ?? default)!
        );

        ValueComparer<T> comparer = new(
            (l, r) => JsonSerializer.Serialize(l, settings) == JsonSerializer.Serialize(r, settings),
            v => v == null ? 0 : JsonSerializer.Serialize(v, settings).GetHashCode(),
            v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, settings), settings)!
        );

        propertyBuilder.HasConversion(converter);
        propertyBuilder.Metadata.SetValueConverter(converter);
        propertyBuilder.Metadata.SetValueComparer(comparer);
        propertyBuilder.HasColumnType("jsonb");

        return propertyBuilder;
    }
}