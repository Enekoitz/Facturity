using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints;

public static class ErrorExtensions
{
    /// <summary>
    /// Mapear elementos del enumerador ErrorType a códigos HTTP adecuados.
    /// </summary>
    public static int ToHttpStatusCode(this ErrorType errorType) => errorType switch
    {
        ErrorType.None => 200,             // 200 OK
        ErrorType.Validation => 400,       // 400 Bad Request
        ErrorType.Unauthorized => 401,     // 401 Unauthorized
        ErrorType.NotFound => 404,         // 404 Not Found
        ErrorType.Conflict => 409,         // 409 Conflict
        ErrorType.Business => 422,         // 422 Unprocessable Entity
        ErrorType.Unexpected => 500,       // 500 Internal Server Error
        _ => 500                           // Por defecto ante cualquier valor desconocido
    };
    
    /// <summary>
    /// Obtener el código de error prioritario (por severidad) de una lista de errores.
    /// </summary>
    public static int GetPrimaryHttpStatusCode(this IEnumerable<Error> errors)
    {      
        var errorList = errors as List<Error> ?? errors.ToList();

        // Evaluamos en orden de precedencia estricto
        if (errorList.Any(e => e.Type == ErrorType.Unexpected))
            return 500;

        if (errorList.Any(e => e.Type == ErrorType.Unauthorized))
            return 401;

        if (errorList.Any(e => e.Type == ErrorType.NotFound))
            return 404;

        if (errorList.Any(e => e.Type == ErrorType.Conflict))
            return 409;

        if (errorList.Any(e => e.Type == ErrorType.Business))
            return 422;

        if (errorList.Any(e => e.Type == ErrorType.Validation))
            return 400;

        return 400; // Por defecto ante errores no mapeados
    }
}