using Facturity.Api.Helpers;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class CrearClienteEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateCustomerCommand command,
                                        ICommandHandler<CreateCustomerCommand, Result<Guid>> handler,
                                        HttpContext httpContext,
                                        CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(command, ct);
                
                return result.IsSuccess
                    ? Results.Created($"/api/customers/{result.Value}", new { Id = result.Value })
                    : result.ToProblemDetails(httpContext);
            })
            .RequireAuthorization()
            .WithName("CrearCliente")
            .WithSummary("Crear nuevo customer")
            .WithDescription("Crea y guarda un nuevo customer.")
            .WithValidation<CreateCustomerCommand>();
    }
}
