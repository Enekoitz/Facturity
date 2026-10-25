using Microsoft.EntityFrameworkCore;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Abstractions;

namespace Facturity.Modules.Customer.Infrastructure;

internal  class CustomerRepository(ClientesDbContext context, ITaxIdFactory taxIdFactory) 
    : ICustomerRepository
{

    public async Task<bool> AliasInUseAsync(
        string alias, 
        CancellationToken ct = default)
    {
        return await context.Clientes
                                            .AnyAsync(c => c.Alias == alias 
                                                           && c.DeletedAtUtc == null, cancellationToken: ct);
    }

    public async Task<bool> TaxIdInUseAsync(
        string value, 
        TaxIdType type, 
        string? countryCode, 
        CancellationToken ct = default)
    {
        return await context.Clientes
                                            .AnyAsync(c => c.TaxIdValue == value 
                                                           && c.TaxIdType == type 
                                                           && c.TaxIdCountryCode == countryCode
                                                           && c.DeletedAtUtc == null, cancellationToken: ct);
    }

    public async Task CreateAsync(
        Domain.Customer customer, 
        CancellationToken ct = default)
    {
        var customerRecord = customer.ToRecord();
        await context.Clientes.AddAsync(customerRecord, ct);
        await context.SaveChangesAsync(ct);
    }
    
    public async Task UpdateAsync(
        Domain.Customer customer, 
        CancellationToken ct = default)
    {
        var customerRecord = customer.ToRecord();
        context.Clientes.Update(customerRecord);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Domain.Customer?> GetByIdAsync(
        Guid id, 
        CancellationToken ct = default)
    {
        var customerRecord = await context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: ct);

        if (customerRecord == null)
            return null; 

        var customer = customerRecord.FromRecord(taxIdFactory)
            ?? throw new InvalidOperationException($"El registro con ID '{id}' tiene un estado corrupto o no válido para el dominio.");

        return customer;
    }
    
    public async Task<SoftDeleteResult> SoftDeleteAsync(
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

        var customer = await context.Clientes
            .Where(c => c.Id == id)
            .Select(c => new { c.DeletedAtUtc })
            .SingleOrDefaultAsync(cancellationToken: ct);
        
        return customer?.DeletedAtUtc != null 
            ? SoftDeleteResult.AlreadyDeleted(customer.DeletedAtUtc.Value) 
            : SoftDeleteResult.NotFound();
    }

    public async Task ActivateAsync(
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

    public async Task DeactivateAsync(
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
    
    public async Task<IEnumerable<Domain.Customer>> GetAsync(
        GetCustomersQuery query, 
        CancellationToken ct = default)
    {
        int initPosition = (query.Page - 1) * query.Size;

        var queryable = context.Clientes.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Alias))
            queryable = queryable
                .Where(c => EF.Functions.Like(c.Alias.ToUpper(), $"%{query.Alias.ToUpper()}%") );
        
        if(query.TaxIdType != null)
            queryable = queryable
                .Where(c => c.TaxIdType == query.TaxIdType);
        
        if (!string.IsNullOrWhiteSpace(query.TaxIdValue))
            queryable = queryable
                .Where(c =>  EF.Functions.Like(c.TaxIdValue.ToUpper(), 
                                                    $"%{query.TaxIdValue.ToUpper()}%") );
            
        if (!string.IsNullOrWhiteSpace(query.TaxIdCountryCode))
            queryable = queryable
                .Where(c => EF.Functions.Like(c.TaxIdCountryCode.ToUpper(), $"%{query.TaxIdCountryCode.ToUpper()}%") );
        
        if (!string.IsNullOrWhiteSpace(query.TaxNameSearch))
            queryable = queryable
                .Where(c => EF.Functions.Like(c.TaxNameSearch.ToUpper(), $"%{query.TaxNameSearch.ToUpper()}%")  );
        
        if (!string.IsNullOrWhiteSpace(query.AddressSearch))
            queryable = queryable
                .Where(c => EF.Functions.Like(c.AddressSearch.ToUpper(), $"%{query.AddressSearch.ToUpper()}%")  );

        if(query.Active.HasValue)
            queryable = queryable
                .Where(c =>  query.Active == true ? c.ActivatedAtUtc != null : c.DeactivatedAtUtc != null);
        
        var clienteRecords = await queryable              
            .OrderBy(c => c.Id)
            .Skip(initPosition)
            .Take(query.Size)
            .ToListAsync(ct);
        
        return clienteRecords
            .Select(c => c.FromRecord(taxIdFactory)).ToList()!;
    }
}
