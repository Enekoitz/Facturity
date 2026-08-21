using Microsoft.EntityFrameworkCore;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using System.Data.SqlTypes;

namespace Facturity.Modules.Customer.Infrastructure;

public class ClientesRepository(ClientesDbContext context, IIdentificacionFiscalFactory identificacionFiscalFactory) 
    : IClientesRepository
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

    public async Task GuardarNuevoClienteAsync(Cliente cliente, CancellationToken ct = default)
    {
        var clienteRecord = cliente.ToRecord();
        await _context.Clientes.AddAsync(clienteRecord, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Cliente?> ObtenerClientePorIdAsync(Guid id, CancellationToken ct = default)
    {
        var clienteRecord = await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: ct);

        if (clienteRecord == null)
            return null; 

        var cliente = clienteRecord.FromRecord(identificacionFiscalFactory)
            ?? throw new InvalidOperationException($"El registro con ID '{id}' tiene un estado corrupto o no válido para el dominio.");

        return cliente;
    }
}
