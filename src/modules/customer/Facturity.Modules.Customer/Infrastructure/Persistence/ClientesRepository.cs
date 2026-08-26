using Microsoft.EntityFrameworkCore;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Facturity.Modules.Customer.Infrastructure;

public class ClientesRepository(ClientesDbContext context, IIdentificacionFiscalFactory identificacionFiscalFactory) 
    : IClientesRepository
{

    public async Task<bool> AliasEnUsoAsync(string alias, CancellationToken ct = default)
    {
        var resultado = await context.Clientes.AnyAsync(c => c.Alias == alias, cancellationToken: ct);
        return resultado;
    }

    public async Task<bool> IdentificacionFiscalEnUsoAsync(string valor, TiposIdentificacionFiscal tipo, string? codigoPais, CancellationToken ct = default)
    {
        var resultado = await context.Clientes.AnyAsync(c => c.IdentificacionFiscalValor == valor && c.IdentificacionFiscalTipo == tipo && c.IdentificacionFiscalCodigoPais == codigoPais, cancellationToken: ct);
        return resultado;
    }

    public async Task GuardarNuevoClienteAsync(Cliente cliente, CancellationToken ct = default)
    {
        var clienteRecord = cliente.ToRecord();
        await context.Clientes.AddAsync(clienteRecord, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Cliente?> ObtenerClientePorIdAsync(Guid id, CancellationToken ct = default)
    {
        var clienteRecord = await context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: ct);

        if (clienteRecord == null)
            return null; 

        var cliente = clienteRecord.FromRecord(identificacionFiscalFactory)
            ?? throw new InvalidOperationException($"El registro con ID '{id}' tiene un estado corrupto o no válido para el dominio.");

        return cliente;
    }
    
    public async Task<SoftDeleteResult> EliminarClienteSoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var deletedAtUtc = DateTime.UtcNow;
        
        var affectedRow = await context.Clientes
            .Where(c => c.Id == id && c.DeletedAtUtc == null)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(c => c.DeletedAtUtc, deletedAtUtc), cancellationToken: ct);
        
        if (affectedRow == 1)
            return SoftDeleteResult.Deleted(deletedAtUtc);

        var clienteExistente = await context.Clientes
            .Where(c => c.Id == id)
            .Select(c => new { c.DeletedAtUtc })
            .SingleOrDefaultAsync(cancellationToken: ct);
        
        //
        
        return clienteExistente?.DeletedAtUtc != null 
            ? SoftDeleteResult.AlreadyDeleted(clienteExistente.DeletedAtUtc.Value) 
            : SoftDeleteResult.NotFound();
    }
}
