using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Facturity.Api.Exceptions;

/// <summary>
/// Captura excepción desde cualquier punto no controlado de la aplicación, loggea lo necesario y devuelve 
/// ProblemDetail para la respuesta HTTP
/// <param name="logger"></param>
/// <param name="env"></param>
/// </summary>
internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env) 
    : IExceptionHandler
{
    private readonly IHostEnvironment _env = env;
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        bool logged = false;
        try
        {
            // - Pendiente analizar si merece la pena definir excepción más específica y código de estado no 500

            //httpContext.Response.StatusCode = exception switch
            //{
            //    ApplicationException => StatusCodes.Status400BadRequest,
            //    _ => StatusCodes.Status500InternalServerError
            //};

            var isDev = _env.IsDevelopment();

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = isDev ? exception.GetType().Name : "Error interno del servidor",
                Type = isDev ? exception.GetType().FullName : "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = isDev
                    ? exception.Message
                    : "Se ha producido un error procesando su solicitud. Contacte al administrador con el Trace ID.",
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            logger.LogError(exception: exception, message: "Ha ocurrido una excepción no controlada", args: [problemDetails]);


            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
        finally
        {
            if(logged)
                logger.LogError(exception: exception, message: "Ha ocurrido una excepción no controlada");
        }
    }
}