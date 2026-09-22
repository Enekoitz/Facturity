using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class DesactivarClienteHandler(IClientesRepository repo) 
    : ICommandHandler<DesactivarClienteCommand, Result>
{
    public async Task<Result> HandleAsync(DesactivarClienteCommand command, CancellationToken ct)
    {
        await repo.DesactivarClienteAsync(command.Id, ct);
        return Result.Ok();
    }
}
