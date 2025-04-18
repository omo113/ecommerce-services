using CartService.Application.Commands.CartCommands;
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
}