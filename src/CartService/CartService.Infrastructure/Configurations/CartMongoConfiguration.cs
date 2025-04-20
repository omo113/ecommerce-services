using CartService.Domain.Entities.CartEntity;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace CartService.Infrastructure.Configurations;

public static class CartMongoConfiguration
{
    public static void Configure()
    {
        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true)
        };

        ConventionRegistry.Register(
            "DefaultConventions",
            conventionPack,
            _ => true
        );
        if (!BsonClassMap.IsClassMapRegistered(typeof(Cart)))
        {
            BsonClassMap.RegisterClassMap<Cart>(cm =>
{
    cm.AutoMap();
    cm.SetIgnoreExtraElements(true);

    cm.MapIdProperty(c => c.Id)
        .SetElementName("_id")
        .SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.AscendingGuidGenerator.Instance);
});
        }
    }
}