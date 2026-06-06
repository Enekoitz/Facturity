using UgeFactura.Modules.Clientes.Features;

namespace UgeFactura.Api
{
    public static class CrearClienteEndpoint
    {
        public static void MapCrearCliente(this RouteGroupBuilder group)
        {
            group.MapPost("/api/clientes", async (CrearClienteCommand dto, CrearClienteHandler handler) =>
            {
                var id = await handler.HandleAsync(dto);
                return Results.Created($"/api/clientes/{id}", new { Id = id });
            })
            .WithName("CrearCliente");
        }
    }
}
