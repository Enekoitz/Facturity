namespace Facturity.Api.Helpers;

internal static class HttpResponseExtension
{
    /// <summary>
    /// Returns the title based on the provided HTTP status code
    /// </summary>
    internal static string GetTitle(this int httpStatusCode) => httpStatusCode switch
    {
        StatusCodes.Status500InternalServerError => "Internal Server Error",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status403Forbidden => "Forbidden",
        StatusCodes.Status404NotFound => "Nof Found",
        StatusCodes.Status409Conflict => "Conflict",
        StatusCodes.Status422UnprocessableEntity => "Unprocessable",
        StatusCodes.Status400BadRequest => "Bad Request",
        _ => "Operation error"
    };
}