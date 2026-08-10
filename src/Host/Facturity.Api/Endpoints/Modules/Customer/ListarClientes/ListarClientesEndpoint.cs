using Serilog;
using Facturity.Modules.Customer.Features;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ListarClientesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ListarClientesCommand command,ListarClientesHandler handler) =>
        {
            Log.ForContext<Program>().Information("Creando cliente desde una minimal API, registrando con Serilog en Seq");
            var clientes = await handler.HandleAsync(command);
            return Results.Ok(clientes);
        })
        .WithName("ObtenerClientes");
    }
}
