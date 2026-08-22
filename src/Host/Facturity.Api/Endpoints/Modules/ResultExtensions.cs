using System.Diagnostics;
using Facturity.Modules.SharedKernel.Domain;
using Facturity.Api.Helpers;

namespace Facturity.Api.Endpoints;

internal static class ResultExtensions
{
    /// <summary>
    /// Mapear objeto Result a ProblemDetails para la HTTP response adecuada
    /// </summary>
    internal static IResult ToProblemDetails(
        this IResultBase result,
        HttpContext httpContext)
    {
        // 1. Si el resultado es exitoso o no hay errores guardados se lanza excepción
        if (result.IsSuccess || result.Errors is null || !result.Errors.Any())
        {
            throw new InvalidOperationException("No se puede generar un ProblemDetail de un Result exitoso.");
        }
          
        // 2. Obtener el error y su código de estado HTTP correspondiente con mayor prioridad de severidad
        var mostSeverePriorityError = GetMostSeverePriorityError(result.Errors);
        int primaryStatusCode = GetStatusCode(mostSeverePriorityError.Type);
                  
        // 3. Obtener el texto para el 'detail' en base al tipo de error más prioritario
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
                  
        // 4. Devolver la respuesta HTTP
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
    /// Obtener el error con mayor prioridad de severidad ordenando los errores en base a las reglas de prioridad y devolviendo el primer error
    /// </summary>
    private static Error GetMostSeverePriorityError(IReadOnlyCollection<Error> errors)
    {
        return errors.OrderBy(e => GetPriorityWeight(e.Type)).First();
    }

    /// <summary>
    /// Orden de prioridad de severidad de los errores de un Result
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
    /// Obtener el HTTP status code del tipo de error proporcionado
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