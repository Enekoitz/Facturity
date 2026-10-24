using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;

namespace Facturity.Modules.Customer.Features;

internal interface IClientesRepository
{
    Task<bool> AliasEnUsoAsync(string alias, CancellationToken ct = default);

    Task<bool> IdentificacionFiscalEnUsoAsync(string valor, TaxIdType tipo, string? codigoPais, CancellationToken ct = default);

    Task GuardarAsync(Domain.Customer customer, CancellationToken ct = default);
    Task ActualizarAsync(Domain.Customer customer, CancellationToken ct = default);

    Task<Domain.Customer?> ObtenerClientePorIdAsync(Guid id, CancellationToken ct = default);
    
    Task<SoftDeleteResult> EliminarClienteSoftDeleteAsync(Guid id, CancellationToken ct = default);
    
    Task ActivarClienteAsync(Guid id, CancellationToken ct = default);
    
    Task DesactivarClienteAsync(Guid id, CancellationToken ct = default);
    
    Task<IEnumerable<Domain.Customer>> ListarClientesAsync(ListarClientesQuery query, CancellationToken ct = default);
}