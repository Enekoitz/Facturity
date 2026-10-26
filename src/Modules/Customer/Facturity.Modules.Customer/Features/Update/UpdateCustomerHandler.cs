using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class UpdateCustomerHandler(ICustomerRepository repo, ITaxIdFactory taxIdFactory) 
    : ICommandHandler<UpdateCustomerCommand, Result>
{
    public async Task<Result> HandleAsync(UpdateCustomerCommand command, CancellationToken ct)
    {
        var customer = await repo.GetByIdAsync(command.Id, ct);
        if (customer == null)
            return Result.Fail(CustomerErrors.NotFound);

        Result<ITaxId> taxIdResult = default;
        if (command.TaxIdValue is not null && command.TaxIdType is not null)
        {
            taxIdResult = taxIdFactory.Create(
                command.TaxIdValue, 
                command.TaxIdType.Value, 
                command.TaxIdCountryCode)!;
            
            if (taxIdResult.IsFailure)
                return Result.Fail(taxIdResult.Errors);
            
            var taxIdInUse = await repo.TaxIdInUseAsync(
                command.TaxIdValue,
                command.TaxIdType.Value, 
                command.TaxIdCountryCode, 
                ct)!;
            
            if (taxIdInUse)
                return Result.Fail(CustomerErrors.TaxIdInUse);
        }

        Result<Address> addressResult = default;
        if (command.TaxIdValue is not null && command.TaxIdType is not null)
        {
            addressResult = Address.Create(command.Street!, command.PostalCode, command.City, 
                command.State, command.Region, command.Country);
        
            if (addressResult.IsFailure)
                return Result.Fail(addressResult.Errors);
        }

        var taxId = taxIdResult == default ? null :  taxIdResult.Value;
        var address = addressResult == default ? null : addressResult.Value;
        
        var result = customer.Update(taxId, command.LegalEntityName,
                                    command.Name, command.FirstSurname, command.SecondSurname, 
                                    address);
        
        if (result.IsFailure)
            return result;

        await repo.UpdateAsync(customer, ct);

        return Result.Ok();
    }
}
