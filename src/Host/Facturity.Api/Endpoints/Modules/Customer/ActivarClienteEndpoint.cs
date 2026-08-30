using Facturity.Api.Helpers;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ActivarClienteEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/{id}/activar", async ([AsParameters] ActivarClienteCommand command,
                                            ICommandHandler<ActivarClienteCommand, Result> handler,
                                            HttpContext httpContext,
                                            CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(command, ct);
                
                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails(httpContext);
            })
            .WithName("ActivarCliente")
            .WithSummary("Activar cliente")
            .WithDescription("Cambia el estado del cliente a activo y registra la fecha de activación.")
            .WithValidation<ActivarClienteCommandValidator>();
    }
}
