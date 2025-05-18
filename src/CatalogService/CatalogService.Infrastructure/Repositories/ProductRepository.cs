using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CatalogService.Domain.Aggregates.ProductEntity;

namespace CatalogService.Infrastructure.Repositories;

public class ProductRepository(CatalogDbContext dbContext) :
    EfRepository<Product>(dbContext), IProductRepository
{
    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await DbContext.Products.Include(p => p.Category).Include(x => x.Price).FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Product>> ListAsync(Expression<Func<Product, bool>> predicate)
    {
        return await DbContext.Products
            .Include(p => p.Category)
            .Include(x => x.Price)
            .Where(predicate)
            .ToListAsync();
    }

}