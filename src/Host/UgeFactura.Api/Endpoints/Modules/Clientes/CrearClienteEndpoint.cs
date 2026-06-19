using Microsoft.AspNetCore.Mvc;
using UgeFactura.Modules.Clientes.Features;
namespace UgeFactura.Api
{
    public class CrearClienteEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                [FromBody] CrearClienteCommand command, 
                [FromServices] CrearClienteHandler handler) =>
            {
                var id = await handler.HandleAsync(command);
                return Results.Created($"/api/clientes/{id}", new { Id = id });
            })
            .WithName("CrearCliente");
        }
    }
}
