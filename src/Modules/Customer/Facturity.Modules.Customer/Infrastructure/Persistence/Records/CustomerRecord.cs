using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public sealed class CustomerRecord
{
    public Guid Id { get; init; }
    public string Alias { get; init; }
    public string TaxIdValue { get; init; }
    public TaxIdType TaxIdType { get; init; }
    public string TaxIdCountryCode { get; init; }
    public string? LegalEntityName { get; init; }
    public string? Name { get; init; }
    public string? FirstSurname { get; init; }
    public string? SecondSurname { get; init; }
    public Address? Address { get; init; }
    public DateTimeOffset? ActivatedAtUtc { get; init; }
    public DateTimeOffset? DeactivatedAtUtc { get; init; }
    public DateTimeOffset? DeletedAtUtc { get; init; }
    public string AddressSearch { get; init; }
    public string TaxNameSearch { get; init; }

    private CustomerRecord() {}

    public CustomerRecord(
        Guid id,
        string alias,
        string taxIdValue,
        TaxIdType taxIdType,
        string taxIdCountryCode,
        string? legalEntityName,
        string? firstName,
        string? firstSurname,
        string? secondSurname,
        Address? address,
        DateTimeOffset? activatedAtUtc,
        DateTimeOffset? deactivatedAtUtc,
        DateTimeOffset? deletedAtUtc)
    {
        Id = id;
        Alias = alias;
        TaxIdValue = taxIdValue;
        TaxIdType = taxIdType;
        TaxIdCountryCode =  taxIdCountryCode;
        LegalEntityName = legalEntityName;
        Name = firstName;
        FirstSurname = firstSurname;
        SecondSurname = secondSurname;
        Address = address;
        ActivatedAtUtc = activatedAtUtc;
        DeactivatedAtUtc = deactivatedAtUtc;
        DeletedAtUtc = deletedAtUtc;
    }
}