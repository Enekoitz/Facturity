using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

public sealed class ListarClientesHandler(IClientesRepository repoCliente)
{
    private readonly IClientesRepository _repoCliente = repoCliente;

    public async Task<Result<Guid>> HandleAsync(ListarClientesQuery query)
    {
        // 1. Hay que listar los clientes de un tenant concreto siempre.
        // 
        throw new NotImplementedException();
    }
}
