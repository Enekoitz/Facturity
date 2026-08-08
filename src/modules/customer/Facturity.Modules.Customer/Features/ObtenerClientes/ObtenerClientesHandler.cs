using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

public sealed class ObtenerClientesHandler
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
