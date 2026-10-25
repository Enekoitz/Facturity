using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class DeactivateCustomerHandler(ICustomerRepository repo) 
    : ICommandHandler<DeactivateCustomerCommand, Result>
{
    public async Task<Result> HandleAsync(DeactivateCustomerCommand command, CancellationToken ct)
    {
        await repo.DeactivateAsync(command.Id, ct);
        return Result.Ok();
    }
}
