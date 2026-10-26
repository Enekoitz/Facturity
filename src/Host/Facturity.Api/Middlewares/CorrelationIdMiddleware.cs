using Serilog.Context;

namespace Facturity.Api.Middlewares;

/// <summary>
/// Middleware that manages the correlation ID in incoming requests.
/// </summary>
public class CorrelationIdMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    private const string CorrelationIdHeader = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
    {
        var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var existingId) 
                            && !string.IsNullOrWhiteSpace(existingId)
            ? existingId.ToString() 
            : Guid.NewGuid().ToString().ToUpper();

        context.Items[CorrelationIdHeader] = correlationId;

        context.Response.Headers[CorrelationIdHeader] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}

/// <summary>
/// Extension methods for enabling <see cref="CorrelationIdMiddleware"/>.
/// </summary>
public static class CorrelationIdMiddlewareExtensions
{
    /// <summary>
    /// Add a middleware to the pipeline that will handle the correlation ID in incoming requests.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationIdMiddleware>();
    }
}