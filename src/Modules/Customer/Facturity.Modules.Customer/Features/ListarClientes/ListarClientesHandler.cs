using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class ListarClientesHandler(IClientesRepository repo) 
    : IQueryHandler<ListarClientesQuery, Result<IEnumerable<Domain.Customer>>>
{
    public async Task<Result<IEnumerable<Domain.Customer>>> HandleAsync(ListarClientesQuery query, CancellationToken ct = default)
    {
        var clientes = await repo.ListarClientesAsync(query, ct);
        return Result.Ok(clientes);
    }
}
