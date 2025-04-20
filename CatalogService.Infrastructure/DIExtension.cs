using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Persistance;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<CatalogDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("catalog-db")));
        return services;

    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}