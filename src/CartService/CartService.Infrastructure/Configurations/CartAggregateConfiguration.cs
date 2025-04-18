using CartService.Domain.Aggregates;
using MongoDB.Bson.Serialization;

namespace CartService.Infrastructure.Configurations;

public static class CartAggregateConfiguration
{
    public static void Configure()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(CartAggregate)))
        {
            BsonClassMap.RegisterClassMap<CartAggregate>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);

                cm.MapIdProperty(c => c.Id)
                    .SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.AscendingGuidGenerator.Instance)
                    .SetElementName("_id");
            });

            BsonClassMap.RegisterClassMap<Image>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Money>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}