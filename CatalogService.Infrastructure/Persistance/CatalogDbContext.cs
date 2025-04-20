using CatalogService.Domain.Entities.CategoryEntity;
using CatalogService.Domain.Entities.ProductEntity;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistance;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}