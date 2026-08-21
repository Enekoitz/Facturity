using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Infrastructure;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class ObtenerClientePorIdHandler(IClientesRepository repoCliente, IIdentificacionFiscalFactory identificacionFiscalFactory) 
    : IQueryHandler<ObtenerClientePorIdQuery, Result<Cliente>>
{
    private readonly IClientesRepository _repoCliente = repoCliente;
    private readonly IIdentificacionFiscalFactory _identificacionFiscalFactory = identificacionFiscalFactory;

    public async Task<Result<ClienteDto>> HandleAsync(ObtenerClienteQuery request, CancellationToken ct)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(request.Id, ct);

        if (cliente is null)
            return Result<ClienteDto>.Failure("Cliente no encontrado"); // Controlado (404)

        return Result<ClienteDto>.Success(cliente.ToDto());
    }
}
