using System.Diagnostics;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Helpers;

internal static class ResultToHttpExtensions
{
    /// <summary>
    /// Map the Result object to ProblemDetails for the appropriate HTTP response
    /// </summary>
    internal static IResult ToProblemDetails(
        this IResultBase result,
        HttpContext httpContext)
    {
        if (result.IsSuccess || result.Errors is null || !result.Errors.Any())
        {
            throw new InvalidOperationException("No se puede generar un ProblemDetail de un Result exitoso.");
        }
          
        var mostSeverePriorityError = GetMostSeverePriorityError(result.Errors);
        int primaryStatusCode = GetStatusCode(mostSeverePriorityError.Type);
                  
        string detailText = mostSeverePriorityError.Type switch
        {
            ErrorType.Validation => result.Errors.Count == 1 
                ? mostSeverePriorityError.Description 
                : $"Se encontraron {result.Errors.Count} errores de validación en la solicitud.",
          
            ErrorType.NotFound or 
            ErrorType.Conflict or 
            ErrorType.Forbidden or 
            ErrorType.Unauthorized or 
            ErrorType.Business => mostSeverePriorityError.Description,
          
            _ => "Ocurrió un error interno al procesar la solicitud. Inténtelo más tarde."
        };
                  
        return TypedResults.Problem(
            statusCode: primaryStatusCode,
            title: primaryStatusCode.GetTitle(),
            detail: detailText,
            instance: httpContext.Request.Path,
            extensions: new Dictionary<string, object?>
            {
                ["errors"] =  result.Errors.Select(e => new { e.Code, e.Description, Type = e.Type.ToString() }),
                ["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier
            }
        );
    }

    /// <summary>
    /// Get the error with the highest severity priority by sorting the errors according to priority rules and returning the first error.
    /// </summary>
    private static Error GetMostSeverePriorityError(IReadOnlyCollection<Error> errors)
    {
        return errors.OrderBy(e => GetPriorityWeight(e.Type)).First();
    }

    /// <summary>
    /// Order of priority of severity of errors in a Result
    /// </summary>
    private static int GetPriorityWeight(ErrorType errorType) => errorType switch
    {
        ErrorType.Unexpected => 1,
        ErrorType.Unauthorized => 2,
        ErrorType.Forbidden => 3,
        ErrorType.NotFound => 4,
        ErrorType.Conflict => 5,
        ErrorType.Business => 6,
        ErrorType.Validation => 7,
        _ => 8
    };

    /// <summary>
    /// Obtain the HTTP status code for the provided error type
    /// </summary>
    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Business => StatusCodes.Status422UnprocessableEntity,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status400BadRequest
    };
}