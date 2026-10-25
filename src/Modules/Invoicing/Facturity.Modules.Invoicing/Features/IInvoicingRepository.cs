using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Abstractions;

namespace Facturity.Modules.Invoicing.Features;

internal interface IInvoicingRepository
{
    // Task<bool> AliasEnUsoAsync(string alias, CancellationToken ct = default);
    //
    // Task<bool> IdentificacionFiscalEnUsoAsync(string valor, TiposIdentificacionFiscal tipo, string? countryCode, CancellationToken ct = default);
    //
    // Task GuardarAsync(Cliente customer, CancellationToken ct = default);
    // Task ActualizarAsync(Cliente customer, CancellationToken ct = default);
    //
    // Task<Cliente?> ObtenerClientePorIdAsync(Guid id, CancellationToken ct = default);
    //
    // Task<SoftDeleteResult> EliminarClienteSoftDeleteAsync(Guid id, CancellationToken ct = default);
    //
    // Task ActivarClienteAsync(Guid id, CancellationToken ct = default);
    //
    // Task DesactivarClienteAsync(Guid id, CancellationToken ct = default);
    //
    // Task<IEnumerable<Cliente>> ListarClientesAsync(ListarClientesQuery query, CancellationToken ct = default);
}