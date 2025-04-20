using CartService.Domain.Entities.CartEntity;

namespace CartService.Domain.Repositories;

public interface ICartRepository
{
    Task<bool> DeleteCartItem(Cart cart, CancellationToken cancellationToken);
    Task<List<Cart>> GetAll(CancellationToken cancellationToken);
    Task<bool> IdExists(string id, CancellationToken cancellationToken);
    Task CreateCartAsync(Cart cart, CancellationToken cancellationToken);
    Task<Cart?> GetCartById(string id, CancellationToken cancellationToken);
}