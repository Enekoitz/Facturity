using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public static class CustomerMapper
{
    public static CustomerRecord ToRecord(this Domain.Customer customer)
    {

        var (value, type, countryCode) = customer.TaxId.GetCommonValues();

        return new CustomerRecord(
            customer.Id,
            customer.Alias,
            value,
            type,
            countryCode,
            customer.LegalEntityName,
            customer.Name,
            customer.FirstSurname,
            customer.SecondSurname,
            customer.Address,
            customer.ActivatedAtUtc,
            customer.DeactivatedAtUtc,
            customer.DeletedAtUtc);
    }

    public static Domain.Customer? FromRecord(this CustomerRecord customerRecord, ITaxIdFactory taxIdFactory)
    {

        var taxIdResult = taxIdFactory.Create(customerRecord.TaxIdValue, 
            customerRecord.TaxIdType, customerRecord.TaxIdCountryCode);
        
        if (taxIdResult.IsFailure)
            return null;

        var tenantIdResult = TenantId.Create(Guid.NewGuid());

        var customerResult = Domain.Customer.Rebuild(
            customerRecord.Id, 
            tenantIdResult.Value, 
            customerRecord.Alias, 
            taxIdResult.Value, 
            customerRecord.LegalEntityName,
            customerRecord.Name, 
            customerRecord.FirstSurname, 
            customerRecord.SecondSurname,
            customerRecord.Address, 
            customerRecord.ActivatedAtUtc,
            customerRecord.DeactivatedAtUtc, 
            customerRecord.DeletedAtUtc, 
            TimeProvider.System
        );

        return customerResult.IsFailure 
            ? null 
            : customerResult.Value;
    }
}
