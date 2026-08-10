using Microsoft.EntityFrameworkCore;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using System.Data.SqlTypes;

namespace Facturity.Modules.Customer.Infrastructure;

public class ClientesRepository(ClientesDbContext context) : IClientesRepository
{
    private readonly ClientesDbContext _context = context;

    public async Task<bool> ExisteAliasAsync(string alias, CancellationToken ct = default)
    {
        var resultado = await _context.Clientes.AnyAsync(c => c.Alias == alias, cancellationToken: ct);
        return resultado;
    }

    public async Task<bool> ExisteIdentificacionFiscalAsync(string valor, string tipo, string? codigoPais, CancellationToken ct = default)
    {
        var resultado = await _context.Clientes.AnyAsync(c => c.Nif == valor || (c.IdOtroExtranjeroValor == valor && c.IdOtroExtranjeroTipo == subTipo && c.IdOtroExtranjeroCodigoPais == codigoPais), cancellationToken: ct);
        return resultado;
    }

    public async Task GuardarAsync(Cliente cliente, CancellationToken ct = default)
    {
        var clienteRecord = cliente.ToRecord();
        await _context.Clientes.AddAsync(clienteRecord, ct);
    }

    //public async Task<TipoIdentificacionFiscal?> ObtenerTipoIdentificacionAsync(string codigoTipoIdentificacion, CancellationToken cancellationToken = default)
    //{
    //    var resultado = await _context.TipoIdentificacionFiscal.FirstOrDefaultAsync(e => e.Codigo == codigoTipoIdentificacion, cancellationToken: cancellationToken);
    //    return resultado;
    //}
}
