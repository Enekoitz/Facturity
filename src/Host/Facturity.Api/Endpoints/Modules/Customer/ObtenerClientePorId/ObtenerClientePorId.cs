using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Facturity.Api.Endpoints;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ObtenerClientePorId : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CrearClienteCommand command,
                                        ICommandHandler<CrearClienteCommand, Result<Guid>> handler,
                                        CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(command, ct);
                
                return result.IsSuccess
                    ? Results.Created($"/api/clientes/{result.Value}", new { Id = result.Value })
                    : result.ToProblemDetails();
            })
            .WithName("CrearCliente")
            .WithValidation<CrearClienteCommand>();
    }
}
