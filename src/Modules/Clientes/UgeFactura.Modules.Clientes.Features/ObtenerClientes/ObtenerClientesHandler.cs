using UgeFactura.Modules.DomainSharedKernel;

namespace UgeFactura.Modules.Clientes.Features;

internal sealed class ObtenerClientesHandler
{
    private readonly IClientesRepository _repoCliente;

    public ObtenerClientesHandler(IClientesRepository repoCliente)
    {
        _repoCliente = repoCliente;
    }

    public async Task<Result<Guid>> HandleAsync(ObtenerClientesCommand command)
    {

        throw new NotImplementedException();
    }
}
