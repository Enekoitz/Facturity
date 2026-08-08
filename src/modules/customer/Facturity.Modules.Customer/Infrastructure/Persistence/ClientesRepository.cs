using Microsoft.EntityFrameworkCore;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.Customer.Infrastructure.Persistence.Mappers;

namespace Facturity.Modules.Customer.Infrastructure.Persistence;

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
        //var resultado = await _context.Clientes.AnyAsync(c => c.Identificacion.Tipo.Codigo == tipoCodigo && c.Identificacion.Valor == valor, cancellationToken: cancellationToken);
        var resultado = await _context.Clientes.AnyAsync(c => c.Identificacion.Valor == valor, cancellationToken: cancellationToken);
        return resultado;
    }

    public async Task GuardarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        var clienteRecord = cliente.ToRecord();
        await _context.Clientes.AddAsync(cliente, cancellationToken);
    }

    //public async Task<TipoIdentificacionFiscal?> ObtenerTipoIdentificacionAsync(string codigoTipoIdentificacion, CancellationToken cancellationToken = default)
    //{
    //    var resultado = await _context.TipoIdentificacionFiscal.FirstOrDefaultAsync(e => e.Codigo == codigoTipoIdentificacion, cancellationToken: cancellationToken);
    //    return resultado;
    //}
}
