using CartService.Domain.Repositories;
using CartService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICartRepository, CartRepository>()
        return services;
    }
}