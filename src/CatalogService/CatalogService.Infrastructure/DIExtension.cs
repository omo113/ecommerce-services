using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Kafka;
using CatalogService.Infrastructure.Persistance;
using CatalogService.Infrastructure.Repositories;
using CatalogService.Infrastructure.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CatalogService.Infrastructure;

public static class DIExtension
{
    public static IHostApplicationBuilder AddPostgres(this IHostApplicationBuilder builder)
    {
        //services.AddDbContextPool<CatalogDbContext>(options => options.UseNpgsql(
        //    configuration.GetConnectionString("catalog-db")));
        builder.AddNpgsqlDbContext<CatalogDbContext>("catalog-db");
        return builder;

    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMigrationService, MigrationService>();
        services.AddHostedService<MigrationHostedService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }

    public static IHostApplicationBuilder AddKafka(this IHostApplicationBuilder builder)
    {
        builder.AddKafkaProducer<string, string>(
            "kafka",
            settings =>
            {
                settings.Config.Acks = Acks.None;
                //settings.Config.
            },
            _ =>
            {
            });
        builder.Services.AddSingleton<KafkaEventPublisher>();
        return builder;
    }
}