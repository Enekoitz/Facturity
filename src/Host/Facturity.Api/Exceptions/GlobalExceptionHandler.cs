using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Facturity.Api.Exceptions;

/// <summary>
/// Catch exceptions from any unhandled point in the application, log the necessary information, and return a `ProblemDetail` object for the HTTP response.
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
            // - It remains to be determined whether it is worth defining a more specific exception and a status code other than 500.

            //httpContext.Response.StatusCode = exception switch
            //{
            //    ApplicationException => StatusCodes.Status400BadRequest,
            //    _ => StatusCodes.Status500InternalServerError
            //};

            var isDev = _env.IsDevelopment();

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = isDev ? exception.GetType().Name : "Internal Server Error",
                Type = isDev ? exception.GetType().FullName : "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = isDev
                    ? exception.Message
                    : "An error occurred while processing your request. Please contact the administrator and provide the Trace ID.",
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            logger.LogError(exception: exception, message: "An unhandled exception has occurred", args: [problemDetails]);


            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
        finally
        {
            if(logged)
                logger.LogError(exception: exception, message: "An unhandled exception has occurred");
        }
    }
}