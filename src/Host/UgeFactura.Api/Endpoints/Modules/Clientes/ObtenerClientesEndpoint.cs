using Microsoft.AspNetCore.Mvc;
using Serilog;
using UgeFactura.Modules.Clientes.Features;

namespace UgeFactura.Api;

public class ObtenerClientesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
            [FromBody] ObtenerClientesCommand command, 
            [FromServices] ObtenerClientesHandler handler) =>
        {
            Log.ForContext<Program>().Information("Creando cliente desde una minimal API, registrando con Serilog en Seq");
            var clientes = await handler.HandleAsync(command);
            return Results.Ok(clientes);
        })
        .WithName("ObtenerClientes");
    }
}
