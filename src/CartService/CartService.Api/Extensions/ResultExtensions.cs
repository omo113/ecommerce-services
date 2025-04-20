using EcommerceServices.Shared;
using FluentValidation;
using FluentValidation.Results;
using OneOf;

namespace CartService.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToApiResult<T>(this OneOf<T, Error> result)
    {
        return result.Match(
            Results.Ok,
            error => throw new ValidationException(
                error.ErrorCode,
                new List<ValidationFailure>
                {
                        new(string.Empty, error.Description)
                }
            )
        );
    }
}
