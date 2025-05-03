using CartService.Domain.Entities.CartEntity;
using CartService.Domain.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CartService.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly IMongoCollection<Cart> _collection;

    public CartRepository(IMongoClient mongoClient, IOptions<MongoSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.Database);
        _collection = database.GetCollection<Cart>(
            settings.Value.Collection
        );
    }

    public async Task<Cart?> GetCartById(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Cart>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> DeleteCartItem(
        Cart cart,
        CancellationToken cancellationToken
    )
    {
        var res = await _collection.DeleteOneAsync(
            x => x.Id == cart.Id,
            cancellationToken
        );
        return res.IsAcknowledged;
    }

    public async Task<List<Cart>> GetAll(CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cart>.Filter.Empty;
        var carts = await _collection.Find(filter).ToListAsync(cancellationToken);
        return carts;
    }

    public async Task CreateCartAsync(
        Cart cart,
        CancellationToken cancellationToken = default
    ) =>
        await _collection.InsertOneAsync(
            cart,
            new InsertOneOptions(),
            cancellationToken
        );

    public async Task UpdateCartAsync(Cart cart, CancellationToken cancellationToken)
    {
        var filter = Builders<Cart>.Filter.Eq(x => x.Id, cart.Id);
        await _collection.ReplaceOneAsync(filter, cart, new ReplaceOptions { IsUpsert = true }, cancellationToken);
    }

    public async Task<bool> IdExists(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Cart>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }
}
