namespace Facturity.Api.Helpers;

internal static class HttpResponseExtension
{
    /// <summary>
    /// Devolver título según el código de estado HTTP proporcionado
    /// </summary>
    internal static string GetTitle(this int httpStatusCode) => httpStatusCode switch
    {
        StatusCodes.Status500InternalServerError => "Error interno del servidor",
        StatusCodes.Status401Unauthorized => "No autorizado",
        StatusCodes.Status403Forbidden => "Acceso prohibido",
        StatusCodes.Status404NotFound => "Recurso no encontrado",
        StatusCodes.Status409Conflict => "Conflicto con el estado actual",
        StatusCodes.Status422UnprocessableEntity => "Operación no permitida",
        StatusCodes.Status400BadRequest => "Error de validación",
        _ => "Error en la operación"
    };
}