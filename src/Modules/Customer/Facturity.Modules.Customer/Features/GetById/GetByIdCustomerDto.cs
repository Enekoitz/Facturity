using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record GetByIdCustomerDto(
    string Alias,
    string TaxIdValue,
    TaxIdType TaxIdType,
    string? TaxIdCountryCode,
    string? LegalEntityName,
    string? Name,
    string? FirstSurname,
    string? SecondSurname,
    string? Address,
    string? PostalCode,
    string? City,
    string? State,
    string? Region,
    string? Country) { }

internal static class GetByIdCustomerDtoExtensions
{
    internal static GetByIdCustomerDto ToDto(this Domain.Customer customer)
    {
        var (taxIdValue, taxIdType, taxIdCountryCode) = customer.TaxId.GetCommonValues();
        
        return new GetByIdCustomerDto(
            customer.Alias,
            taxIdValue,
            taxIdType,
            taxIdType == TaxIdType.SpanishTaxId 
                ? null 
                : taxIdCountryCode,
            customer.LegalEntityName,
            customer.Name,
            customer.FirstSurname,
            customer.SecondSurname,
            customer.Address?.Street,
            customer.Address?.PostalCode?.Valor,
            customer.Address?.City,
            customer.Address?.State,
            customer.Address?.Region,
            customer.Address?.Country);
    }
}