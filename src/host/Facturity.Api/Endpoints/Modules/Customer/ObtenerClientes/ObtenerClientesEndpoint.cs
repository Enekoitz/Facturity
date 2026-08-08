using Serilog;
using Facturity.Modules.Customer.Features;

namespace Facturity.Api.Endpoints.Modules.Customer.ObtenerClientes;

public class ObtenerClientesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
            ObtenerClientesCommand command, 
            ObtenerClientesHandler handler) =>
        {
            Log.ForContext<Program>().Information("Creando cliente desde una minimal API, registrando con Serilog en Seq");
            var clientes = await handler.HandleAsync(command);
            return Results.Ok(clientes);
        })
        .WithName("ObtenerClientes");
    }
}
