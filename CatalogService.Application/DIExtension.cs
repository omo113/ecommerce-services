using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CatalogService.Application.Commands.CategoryCommands;

namespace CatalogService.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddCategoryCommand>());
        services.AddFluentValidation([typeof(DIExtension).GetTypeInfo().Assembly]);

        return services;
    }
}