using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class DeleteCustomerHandler(ICustomerRepository repo) 
    : ICommandHandler<DeleteCustomerCommand, Result>
{
    public async Task<Result> HandleAsync(DeleteCustomerCommand command, CancellationToken ct)
    {
        var softDeleteResult = await repo.SoftDeleteAsync(command.Id, ct);

        return softDeleteResult.Result switch
        {
            SoftDeleteResultTypes.AlreadyDeleted => Result.Fail(CustomerErrors.AlreadyDeleted),
            SoftDeleteResultTypes.NotFound => Result.Fail(CustomerErrors.NotFound),
            SoftDeleteResultTypes.Deleted => Result.Ok(),
            _ => Result.Fail(Error.Unexpected("Cliente.Error", "Ha ocurrido un error durante la operación."))
        };
    }
}
