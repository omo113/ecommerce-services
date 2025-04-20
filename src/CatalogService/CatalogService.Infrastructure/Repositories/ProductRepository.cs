using CatalogService.Domain.Entities.ProductEntity;
using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Repositories;

public class ProductRepository(CatalogDbContext dbContext) :
    EfRepository<Product>(dbContext), IProductRepository
{
    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await DbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Product>> ListAllAsync()
    {
        return await DbContext.Products
            .Include(p => p.Category)
            .Include(x => x.Price)
            .ToListAsync();
    }
}