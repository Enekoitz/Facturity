using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ObtenerClientePorIdEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", async (
                [AsParameters] ObtenerClientePorIdQuery query,
                IQueryHandler<ObtenerClientePorIdQuery, Result<ClienteDto>> handler,
                HttpContext httpContext,
                CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(query, ct);
                
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails(httpContext);
            })
            .WithName("ObtenerClientePorId")
            .WithValidation<ObtenerClientePorIdQuery>();
    }
}
