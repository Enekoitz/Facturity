using Microsoft.EntityFrameworkCore;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using System.Data.SqlTypes;

namespace Facturity.Modules.Customer.Infrastructure;

public class ClientesRepository(ClientesDbContext context) : IClientesRepository
{
    private readonly ClientesDbContext _context = context;

    public async Task<bool> AliasEnUsoAsync(string alias, CancellationToken ct = default)
    {
        var resultado = await _context.Clientes.AnyAsync(c => c.Alias == alias, cancellationToken: ct);
        return resultado;
    }

    public async Task<bool> IdentificacionFiscalEnUsoAsync(string valor, TiposIdentificacionFiscal tipo, string? codigoPais, CancellationToken ct = default)
    {
        var resultado = await _context.Clientes.AnyAsync(c => c.IdentificacionFiscalValor == valor && c.IdentificacionFiscalTipo == tipo && c.IdentificacionFiscalCodigoPais == codigoPais, cancellationToken: ct);
        return resultado;
    }

    public async Task GuardarAsync(Cliente cliente, CancellationToken ct = default)
    {
        var clienteRecord = cliente.ToRecord();
        await _context.Clientes.AddAsync(clienteRecord, ct);
    }
}
