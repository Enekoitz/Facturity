using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Facturity.Api;

/// <summary>
/// Catches application global exceptions, handle them in a proper way and logs it with Serilog.
/// <param name="problemDetailsService"></param>
/// <param name="logger"></param>
/// </summary>
internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Ha ocurrido una excepción no controlada");

        httpContext.Response.StatusCode = exception switch
        {
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "Ha ocurrido un error",
                Detail = exception.Message
            }
        };

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}