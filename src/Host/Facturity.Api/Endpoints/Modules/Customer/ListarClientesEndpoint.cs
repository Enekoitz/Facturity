using Facturity.Api.Helpers;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ListarClientesEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
                [AsParameters] GetCustomersRequest request,
                IQueryHandler<GetCustomersQuery, Result<IEnumerable<Facturity.Modules.Customer.Domain.Customer>>> handler,
                HttpContext httpContext,
                CancellationToken ct) =>
        {
            var mapResult = request.MapToQuery();
            if (mapResult.IsFailure)
                return mapResult.ToProblemDetails(httpContext); 
                    
            var result = await handler.HandleAsync(mapResult.Value, ct);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.ToProblemDetails(httpContext);
        })
        .RequireAuthorization()
        .WithName("ListarClientes")
        .WithSummary("Listar customers")
        .WithDescription("Devuelve un listado de customers con datos básicos según los filtros proporcionados.");
    }
}
