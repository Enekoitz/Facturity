namespace UgeFactura.Application.Clientes;

using UgeFactura.Domain;
using UgeFactura.Domain.Clientes;

public interface IClienteRepository
{
    Task<bool> ExisteAliasAsync(string alias, CancellationToken cancellationToken = default);

    Task<bool> ExisteIdentificacionFiscalAsync(
        string tipoCodigo,
        string valor,
        CancellationToken cancellationToken = default);

    Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default);

    Task<TipoIdentificacionFiscal?> GetTipoIdentificacionAsync(string codigoTipoIdentificacion, CancellationToken cancellationToken = default);
}