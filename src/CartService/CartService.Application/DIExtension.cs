using CartService.Application.Commands.CartCommands;
using CartService.Application.Handlers;
using CartService.Domain.Handlers;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CartService.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCartCommand>());
        services.AddFluentValidation([typeof(DIExtension).GetTypeInfo().Assembly]);

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IProductUpdatedHandler, ProductUpdatedHandler>();

        return services;
    }
}