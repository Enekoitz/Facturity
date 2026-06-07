using UgeFactura.Modules.Clientes.Features;

namespace UgeFactura.Api
{
    public class ObtenerClientesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (ObtenerClientesCommand dto, ObtenerClientesHandler handler) =>
            {
                //var id = await handler.HandleAsync(dto);
                //return Results.Created($"/api/clientes/{id}", new { Id = id });
                throw new NotImplementedException();
            })
            .WithName("ObtenerClientes");
        }
    }
}
