using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class ActivarClienteHandler(IClientesRepository repo) 
    : ICommandHandler<ActivarClienteCommand, Result>
{
    public async Task<Result> HandleAsync(ActivarClienteCommand command, CancellationToken ct)
    {
        await repo.ActivarClienteAsync(command.Id, ct);
        return Result.Ok();
    }
}
