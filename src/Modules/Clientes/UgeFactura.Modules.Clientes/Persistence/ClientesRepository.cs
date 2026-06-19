using Microsoft.EntityFrameworkCore;
using UgeFactura.Modules.Clientes.Domain;
using UgeFactura.Modules.Clientes.Features;

namespace UgeFactura.Modules.Clientes.Infrastructure;

public class ClientesRepository(ClientesDbContext context) : IClientesRepository
{
    private readonly ClientesDbContext _context = context;

    public async Task<bool> ExisteAliasAsync(string alias, CancellationToken cancellationToken = default)
    {
        var resultado = await _context.Clientes.AnyAsync(c => c.Alias == alias, cancellationToken: cancellationToken);
        return resultado;
    }

    public async Task<bool> ExisteIdentificacionFiscalAsync(string tipoCodigo, string valor, CancellationToken cancellationToken = default)
    {
        var resultado = await _context.Clientes.AnyAsync(c => c.Identificacion.Tipo.Codigo == tipoCodigo && c.Identificacion.Valor == valor, cancellationToken: cancellationToken);
        return resultado;
    }

    public async Task GuardarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
    }

    public async Task<TipoIdentificacionFiscal?> ObtenerTipoIdentificacionAsync(string codigoTipoIdentificacion, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar persistencia en PostgreSQL
        throw new NotImplementedException("Aún no he programado esto");
    }
}
