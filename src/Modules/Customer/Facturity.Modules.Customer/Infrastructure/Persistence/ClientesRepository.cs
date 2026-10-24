using Microsoft.EntityFrameworkCore;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;

namespace Facturity.Modules.Customer.Infrastructure;

internal  class ClientesRepository(ClientesDbContext context, ITaxIdFactory taxIdFactory) 
    : IClientesRepository
{

    public async Task<bool> AliasEnUsoAsync(
        string alias, 
        CancellationToken ct = default)
    {
        return await context.Clientes
                                            .AnyAsync(c => c.Alias == alias 
                                                           && c.DeletedAtUtc == null, cancellationToken: ct);
    }

    public async Task<bool> IdentificacionFiscalEnUsoAsync(
        string valor, 
        TaxIdType tipo, 
        string? codigoPais, 
        CancellationToken ct = default)
    {
        return await context.Clientes
                                            .AnyAsync(c => c.IdentificacionFiscalValor == valor 
                                                           && c.IdentificacionFiscalTipo == tipo 
                                                           && c.IdentificacionFiscalCodigoPais == codigoPais
                                                           && c.DeletedAtUtc == null, cancellationToken: ct);
    }

    public async Task GuardarAsync(
        Domain.Customer customer, 
        CancellationToken ct = default)
    {
        var clienteRecord = customer.ToRecord();
        await context.Clientes.AddAsync(clienteRecord, ct);
        await context.SaveChangesAsync(ct);
    }
    
    public async Task ActualizarAsync(
        Domain.Customer customer, 
        CancellationToken ct = default)
    {
        var clienteRecord = customer.ToRecord();
        context.Clientes.Update(clienteRecord);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Domain.Customer?> ObtenerClientePorIdAsync(
        Guid id, 
        CancellationToken ct = default)
    {
        var clienteRecord = await context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: ct);

        if (clienteRecord == null)
            return null; 

        var cliente = clienteRecord.FromRecord(taxIdFactory)
            ?? throw new InvalidOperationException($"El registro con ID '{id}' tiene un estado corrupto o no válido para el dominio.");

        return cliente;
    }
    
    public async Task<SoftDeleteResult> EliminarClienteSoftDeleteAsync(
        Guid id, 
        CancellationToken ct = default)
    {
        var deletedAtUtc = DateTimeOffset.UtcNow.UtcDateTime;
        
        var affectedRow = await context.Clientes
            .Where(c => c.Id == id 
                        && c.DeletedAtUtc == null)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(c => c.DeletedAtUtc, deletedAtUtc), cancellationToken: ct);
        
        if (affectedRow == 1)
            return SoftDeleteResult.Deleted(deletedAtUtc);

        var clienteExistente = await context.Clientes
            .Where(c => c.Id == id)
            .Select(c => new { c.DeletedAtUtc })
            .SingleOrDefaultAsync(cancellationToken: ct);
        
        return clienteExistente?.DeletedAtUtc != null 
            ? SoftDeleteResult.AlreadyDeleted(clienteExistente.DeletedAtUtc.Value) 
            : SoftDeleteResult.NotFound();
    }

    public async Task ActivarClienteAsync(
        Guid id, 
        CancellationToken ct = default)
    {
        DateTimeOffset? newActivatedAtUtc = DateTimeOffset.UtcNow.UtcDateTime;
        DateTimeOffset? nullDeactivatedAtUtc = null;

        await context.Clientes
            .Where(c => c.Id == id 
                        && c.DeactivatedAtUtc != null 
                        && c.ActivatedAtUtc == null)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(c => c.ActivatedAtUtc, newActivatedAtUtc)
                .SetProperty(c => c.DeactivatedAtUtc, nullDeactivatedAtUtc), cancellationToken:ct);
    }

    public async Task DesactivarClienteAsync(
        Guid id, 
        CancellationToken ct = default)
    {
        DateTimeOffset? nullActivatedAtUtc = null;
        DateTimeOffset? newDeactivatedAtUtc = DateTimeOffset.UtcNow.UtcDateTime;

        await context.Clientes
            .Where(c => c.Id == id && c.DeactivatedAtUtc == null && c.ActivatedAtUtc != null)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(c => c.ActivatedAtUtc, nullActivatedAtUtc)
                .SetProperty(c => c.DeactivatedAtUtc, newDeactivatedAtUtc), cancellationToken:ct);
    }
    
    public async Task<IEnumerable<Domain.Customer>> ListarClientesAsync(
        ListarClientesQuery query, 
        CancellationToken ct = default)
    {
        int initPosition = (query.Page - 1) * query.Size;

        var queryable = context.Clientes.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Alias))
            queryable = queryable
                .Where(c => EF.Functions.Like(c.Alias.ToUpper(), $"%{query.Alias.ToUpper()}%") );
        
        if(query.IdentificacionFiscalTipo != null)
            queryable = queryable
                .Where(c => c.IdentificacionFiscalTipo == query.IdentificacionFiscalTipo);
        
        if (!string.IsNullOrWhiteSpace(query.IdentificacionFiscalValor))
            queryable = queryable
                .Where(c =>  EF.Functions.Like(c.IdentificacionFiscalValor.ToUpper(), 
                                                    $"%{query.IdentificacionFiscalValor.ToUpper()}%") );
            
        if (!string.IsNullOrWhiteSpace(query.IdentificacionFiscalCodigoPais))
            queryable = queryable
                .Where(c => EF.Functions.Like(c.IdentificacionFiscalCodigoPais.ToUpper(), $"%{query.IdentificacionFiscalCodigoPais.ToUpper()}%") );
        
        if (!string.IsNullOrWhiteSpace(query.DenominacionFiscalBusqueda))
            queryable = queryable
                .Where(c => EF.Functions.Like(c.DenominacionFiscalBusqueda.ToUpper(), $"%{query.DenominacionFiscalBusqueda.ToUpper()}%")  );
        
        if (!string.IsNullOrWhiteSpace(query.DireccionBusqueda))
            queryable = queryable
                .Where(c => EF.Functions.Like(c.DireccionBusqueda.ToUpper(), $"%{query.DireccionBusqueda.ToUpper()}%")  );

        if(query.Activo.HasValue)
            queryable = queryable
                .Where(c =>  query.Activo == true ? c.ActivatedAtUtc != null : c.DeactivatedAtUtc != null);
        
        var clienteRecords = await queryable              
            .OrderBy(c => c.Id)
            .Skip(initPosition)
            .Take(query.Size)
            .ToListAsync(ct);
        
        return clienteRecords
            .Select(c => c.FromRecord(taxIdFactory)).ToList()!;
    }
}
