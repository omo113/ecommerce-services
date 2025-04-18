using CartService.Domain.Aggregates;
using CartService.Domain.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CartService.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private const string CollectionName = "cartAggregate";
    private readonly IMongoCollection<CartAggregate> collection;

    public CartRepository(IMongoClient mongoClient, IOptions<MongoSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.Database);
        collection = database.GetCollection<CartAggregate>(
            settings.Value.Collection
        );
    }

}
