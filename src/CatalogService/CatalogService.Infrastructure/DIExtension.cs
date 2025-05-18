using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.BackgroundProcessors;
using CatalogService.Infrastructure.Kafka;
using CatalogService.Infrastructure.Persistance;
using CatalogService.Infrastructure.Repositories;
using CatalogService.Infrastructure.Services;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CatalogService.Infrastructure;

public static class DIExtension
{
    public static IHostApplicationBuilder AddPostgres(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<DomainEventInterceptor>();

        builder.Services.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<DomainEventInterceptor>());
            options.UseNpgsql(builder.Configuration.GetConnectionString("catalog-db"));
        });
        return builder;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMigrationService, MigrationService>();
        services.AddHostedService<MigrationHostedService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddHostedService<EventPublisher>();
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