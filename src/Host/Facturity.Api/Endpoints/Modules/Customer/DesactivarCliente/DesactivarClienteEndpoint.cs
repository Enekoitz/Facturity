using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class DesactivarClienteEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/{id}/desactivar", async ([AsParameters] DesactivarClienteCommand command,
                                            ICommandHandler<DesactivarClienteCommand, Result> handler,
                                            HttpContext httpContext,
                                            CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(command, ct);
                
                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails(httpContext);
            })
            .WithName("DesactivarCliente")
            .WithValidation<DesactivarClienteCommand>();
    }
}
