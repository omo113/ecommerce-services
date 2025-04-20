using FluentValidation;
using NET.DotnetErrorHandling.ProblemDetails;
using NET.DotnetErrorHandling.ProblemDetails.Interfaces;

namespace CartService.Api.Exceptions;

public class FluentValidationErrorHandlingRule : CommonErrorHandlingRule, IErrorHandlingRule
{
    public override bool DoesMatch(Exception exception) => exception is ValidationException;

    protected override string GetTitle(Exception exception) => exception.GetType().Name;

    protected override string GetDetail(Exception exception, HttpContext context) => "One or more validation errors occurred.";

    protected override int GetStatus(Exception exception) => StatusCodes.Status400BadRequest;

    protected override List<ProblemDetail>? GetInnerErrors(Exception exception, HttpContext context)
    {
        var validationException = exception as ValidationException;

        var uniqueErrors = validationException!.Errors
            .GroupBy(e => e.ErrorMessage)
            .Select(group => group.First());

        return uniqueErrors.Select(ex => new ProblemDetail(ex.PropertyName, ex.ErrorMessage)).ToList();
    }

}
