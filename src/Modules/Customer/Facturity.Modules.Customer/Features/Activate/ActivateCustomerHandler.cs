using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class ActivateCustomerHandler(IClientesRepository repo) 
    : ICommandHandler<ActivateCustomerCommand, Result>
{
    public async Task<Result> HandleAsync(ActivateCustomerCommand command, CancellationToken ct)
    {
        await repo.ActivarClienteAsync(command.Id, ct);
        return Result.Ok();
    }
}
