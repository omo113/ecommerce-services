using CartService.Api.Exceptions;
using NET.DotnetErrorHandling.Middlewares;

namespace CartService.Api.Extensions;

public static class ErrorHandlingExtensions
{
    public static IServiceCollection AddErrorHandlingWithRules(this IServiceCollection services)
        => services.AddErrorHandling()
            .AddErrorHandlingRule<FluentValidationErrorHandlingRule>();
}