using UgeFactura.Modules.Clientes.Domain;

namespace UgeFactura.Modules.Clientes.Features
{
    internal interface IClienteRepository
    {
        Task<bool> ExisteAliasAsync(string alias, CancellationToken cancellationToken = default);

        Task<bool> ExisteIdentificacionFiscalAsync(string tipoCodigo,string valor,CancellationToken cancellationToken = default);

        Task GuardarAsync(Cliente cliente, CancellationToken cancellationToken = default);

        Task<TipoIdentificacionFiscal?> ObtenerTipoIdentificacionAsync(string codigoTipoIdentificacion, CancellationToken cancellationToken = default);
    }
}
