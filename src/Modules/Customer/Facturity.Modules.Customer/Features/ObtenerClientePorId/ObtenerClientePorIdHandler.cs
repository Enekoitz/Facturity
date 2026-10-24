using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Infrastructure;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class ObtenerClientePorIdHandler(
    IClientesRepository repo) 
    : IQueryHandler<ObtenerClientePorIdQuery, Result<ClienteDto>>
{
    public async Task<Result<ClienteDto>> HandleAsync(ObtenerClientePorIdQuery request, CancellationToken ct)
    {
        var cliente = await repo.ObtenerClientePorIdAsync(request.Id, ct);

        return cliente is null 
            ? Result<ClienteDto>.Fail(CustomerErrors.NotFound) 
            : Result<ClienteDto>.Ok(cliente.ToDto());
    }
}
