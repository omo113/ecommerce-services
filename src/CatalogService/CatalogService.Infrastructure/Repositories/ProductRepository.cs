using CatalogService.Domain.Entities.ProductEntity;
using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Persistance;

namespace CatalogService.Infrastructure.Repositories;

public class ProductRepository(CatalogDbContext dbContext) :
    EfRepository<Product>(dbContext), IProductRepository;