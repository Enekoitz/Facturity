using Serilog;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class CrearClienteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CrearClienteCommand command, 
                                ICommandHandler<CrearClienteCommand, Result<Guid>> handler,
                                CancellationToken ct) =>
        {
            Log.ForContext<Program>().Information("Creando cliente desde una minimal API, registrando con Serilog en Seq");
            
            var result = await handler.HandleAsync(command, ct);

            return result.IsSuccess
                    ? Results.Created($"/api/clientes/{result.Value}", new { Id = result.Value })
                    : Results.Problem();
        })
        .WithName("CrearCliente");
    }
}
