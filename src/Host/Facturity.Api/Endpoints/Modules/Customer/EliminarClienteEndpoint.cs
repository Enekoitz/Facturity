using Facturity.Api.Helpers;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class EliminarClienteEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", async ([AsParameters] DeleteCustomerCommand command,
                                            ICommandHandler<DeleteCustomerCommand, Result> handler,
                                            HttpContext httpContext,
                                            CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(command, ct);
                
                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails(httpContext);
            })
            .RequireAuthorization()
            .WithName("EliminarCliente")
            .WithSummary("Eliminar customer existente")
            .WithDescription("Elimina un customer existente.")
            .WithValidation<DeleteCustomerCommandValidator>();
    }
}
