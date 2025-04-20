using EcommerceServices.Shared;
using System.Linq.Expressions;

namespace CatalogService.Domain.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync();
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(int id);
    }
}