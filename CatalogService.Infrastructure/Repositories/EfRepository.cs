using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Persistance;
using EcommerceServices.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly CatalogDbContext DbContext;
        protected readonly DbSet<T> DbSet;

        public EfRepository(CatalogDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbSet = DbContext.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ListAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }
    }
}