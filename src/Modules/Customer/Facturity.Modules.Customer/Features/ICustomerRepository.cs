using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;

namespace Facturity.Modules.Customer.Features;

internal interface ICustomerRepository
{
    Task CreateAsync(Domain.Customer customer, CancellationToken ct = default);
    Task UpdateAsync(Domain.Customer customer, CancellationToken ct = default);
    Task<Domain.Customer?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Domain.Customer>> GetAsync(GetCustomersQuery query, CancellationToken ct = default);
    Task<SoftDeleteResult> SoftDeleteAsync(Guid id, CancellationToken ct = default);
    Task ActivateAsync(Guid id, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
    Task<bool> AliasInUseAsync(string alias, CancellationToken ct = default);
    Task<bool> TaxIdInUseAsync(string value, TaxIdType type, string? countryCode, CancellationToken ct = default);
}