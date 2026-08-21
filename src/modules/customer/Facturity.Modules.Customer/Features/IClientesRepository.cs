using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Features;

public interface IClientesRepository
{
    Task<bool> AliasEnUsoAsync(string alias, CancellationToken ct = default);

    Task<bool> IdentificacionFiscalEnUsoAsync(string valor, TiposIdentificacionFiscal tipo, string? codigoPais, CancellationToken ct = default);

    Task GuardarNuevoClienteAsync(Cliente cliente, CancellationToken ct = default);

    Task<Cliente?> ObtenerClientePorIdAsync(Guid id, CancellationToken ct = default);
}