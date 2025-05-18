using CatalogService.Domain.Aggregates.CategoryEntity;
using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Repositories;

public class CategoryRepository(CatalogDbContext dbContext) :
    EfRepository<Category>(dbContext), ICategoryRepository
{
    public override async Task<Category?> GetByIdAsync(int id)
    {
        return await DbContext.Categories.AsQueryable().Where(x => x.Id == id).Include(x => x.Image).FirstOrDefaultAsync();
    }
}