using CartService.Domain.Repositories;
using CartService.Infrastructure.Configurations;
using CartService.Infrastructure.Kafka;
using CartService.Infrastructure.Repositories;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CartService.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(
            configuration.GetSection(nameof(MongoSettings))
        );

        CartMongoConfiguration.Configure();
        services.AddSingleton<ICartRepository, CartRepository>();
        return services;
    }

    public static IHostApplicationBuilder AddMongoClient(this IHostApplicationBuilder builder)
    {
        builder.AddMongoDBClient("carts-db");
        return builder;
    }
    public static IHostApplicationBuilder AddKafka(this IHostApplicationBuilder builder)
    {
        builder.AddKafkaConsumer<string, string>("kafka", configureBuilder =>
        {
            configureBuilder.Config.AutoOffsetReset = AutoOffsetReset.Earliest;
            configureBuilder.Config.EnableAutoOffsetStore = false;
            configureBuilder.Config.GroupId = "cart-service-group";
        });
        builder.Services.AddHostedService<KafkaConsumerService>();
        return builder;
    }
}