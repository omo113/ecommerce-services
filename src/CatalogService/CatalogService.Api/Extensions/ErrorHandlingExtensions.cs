using CartService.Api.Exceptions;
using NET.DotnetErrorHandling.Middlewares;

namespace CatalogService.Api.Extensions;

public static class ErrorHandlingExtensions
{
    public static IServiceCollection AddErrorHandlingWithRules(this IServiceCollection services)
        => services.AddErrorHandling()
            .AddErrorHandlingRule<FluentValidationErrorHandlingRule>();
}