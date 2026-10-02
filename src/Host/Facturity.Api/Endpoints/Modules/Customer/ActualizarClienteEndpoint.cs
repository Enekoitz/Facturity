using Facturity.Api.Helpers;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ActualizarClienteEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/", async (ActualizarClienteCommand command,
                                        ICommandHandler<ActualizarClienteCommand, Result> handler,
                                        HttpContext httpContext,
                                        CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(command, ct);
                
                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails(httpContext);
            })
            .RequireAuthorization()
            .WithName("ActualizarCliente")
            .WithSummary("Actualizar nuevo cliente")
            .WithDescription("Actualiza los datos existentes de un cliente.")
            .WithValidation<ActualizarClienteCommand>();
    }
}
