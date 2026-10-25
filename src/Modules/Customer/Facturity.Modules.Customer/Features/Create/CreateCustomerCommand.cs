using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record CreateCustomerCommand(
    string Alias,
    string TaxIdValue,
    TaxIdType TaxIdType,
    string? TaxIdCountryCode,
    string? LegalEntityName,
    string? Name,
    string? FirstSurname,
    string? SecondSurname,
    string Address,
    string? PostalCode,
    string? City,
    string? State,
    string? Region,
    string? Country
) : ICommand<Result<Guid>>;
