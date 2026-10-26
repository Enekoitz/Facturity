using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record GetCustomersQuery(
    string? Alias, 
    TaxIdType? TaxIdType, 
    string? TaxIdValue, 
    string? TaxIdCountryCode,
    string? TaxNameSearch,
    string? AddressSearch,
    bool? Active,
    int Page,
    int Size) : IQuery<Result<IEnumerable<Domain.Customer>>>;
