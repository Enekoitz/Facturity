using Facturity.Modules.Customer.Features;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ListarClientesEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async ([AsParameters] ListarClientesQuery query,ListarClientesHandler handler) =>
        {
            var clientes = await handler.HandleAsync(query);
            return Results.Ok(clientes);
        })
        .WithName("ObtenerClientes");
    }
}
