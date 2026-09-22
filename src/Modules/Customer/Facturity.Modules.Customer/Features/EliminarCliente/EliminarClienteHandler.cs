using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class EliminarClienteHandler(IClientesRepository repo) 
    : ICommandHandler<EliminarClienteCommand, Result>
{
    public async Task<Result> HandleAsync(EliminarClienteCommand command, CancellationToken ct)
    {
        var softDeleteResult = await repo.EliminarClienteSoftDeleteAsync(command.Id, ct);

        return softDeleteResult.Result switch
        {
            SoftDeleteResultTypes.AlreadyDeleted => Result.Fail(ClienteErrors.EliminadoPreviamente),
            SoftDeleteResultTypes.NotFound => Result.Fail(ClienteErrors.NoEncontrado),
            SoftDeleteResultTypes.Deleted => Result.Ok(),
            _ => Result.Fail(Error.Unexpected("Cliente.Error", "Ha ocurrido un error durante la operación."))
        };
    }
}
