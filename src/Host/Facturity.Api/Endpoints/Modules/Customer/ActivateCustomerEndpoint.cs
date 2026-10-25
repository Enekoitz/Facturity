using Facturity.Api.Helpers;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Api.Endpoints.Modules.Customer;

public class ActivateCustomerEndpoint : IClientesEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/{id}/activate", async ([AsParameters] ActivateCustomerCommand command,
                                                    ICommandHandler<ActivateCustomerCommand, Result> handler,
                                                    HttpContext httpContext,
                                                    CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(command, ct);
                
                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToProblemDetails(httpContext);
            })
            .RequireAuthorization()
            .WithName("ActivateCustomer")
            .WithSummary("Activate a customer")
            .WithDescription("Change the customer's status to active and record the activation date.")
            .WithValidation<ActivateCustomerCommandValidator>();
    }
}
