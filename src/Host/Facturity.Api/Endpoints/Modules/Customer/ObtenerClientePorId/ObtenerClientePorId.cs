using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Facturity.Api.Endpoints;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ObtenerClientePorId : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", async (
                [AsParameters] ObtenerClientePorIdQuery query,
                IQueryHandler<ObtenerClientePorIdQuery, Result<ClienteDto>> handler,
                CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(query, ct);
                
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName("ObtenerClientePorId")
            .WithValidation<ObtenerClientePorIdQuery>();
    }
}
