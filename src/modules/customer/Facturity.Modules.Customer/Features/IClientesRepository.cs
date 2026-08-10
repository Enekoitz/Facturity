using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Features;

public interface IClientesRepository
{
    Task<bool> ExisteAliasAsync(string alias, CancellationToken ct = default);

    Task<bool> ExisteIdentificacionFiscalAsync(string valor, string tipo, string? codigoPais, CancellationToken ct = default);

    Task GuardarAsync(Cliente cliente, CancellationToken ct = default);

    //Task<TipoIdentificacionFiscal?> ObtenerTipoIdentificacionAsync(string codigoTipoIdentificacion, CancellationToken cancellationToken = default);
}