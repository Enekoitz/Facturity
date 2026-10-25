using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class CreateCustomerHandler(ICustomerRepository repo, ITaxIdFactory taxIdFactory) 
    : ICommandHandler<CreateCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(CreateCustomerCommand command, CancellationToken ct)
    {
        var taxIdResult = taxIdFactory.Create(command.TaxIdValue, command.TaxIdType, command.TaxIdCountryCode);
        if (taxIdResult.IsFailure)
            return Result.Fail<Guid>(taxIdResult.Errors);

        var addressResult = Address.Crear(command.Address, command.PostalCode, command.City, 
                                        command.State, command.Region, command.Country);
        if (addressResult.IsFailure)
            return Result.Fail<Guid>(addressResult.Errors);

        // TODO: Pendiente de quitar de aquí e implementar tenantId desde claims con seguridad vía JWT access token, etc.
        var tenantIdResult = TenantId.Crear(Guid.NewGuid());

        var id = Guid.NewGuid();
        var customerResult = Domain.Customer.Create(id, tenantIdResult.Value, command.Alias, taxIdResult.Value, command.LegalEntityName,
                                            command.Name, command.FirstSurname, command.SecondSurname, addressResult.Value);
        if (customerResult.IsFailure)
            return Result.Fail<Guid>(customerResult.Errors);

        if (await repo.AliasInUseAsync(command.Alias, ct))
            return Result.Fail<Guid>(CustomerErrors.AliasInUse);

        if (await repo.TaxIdInUseAsync(command.TaxIdValue, command.TaxIdType, command.TaxIdCountryCode, ct))
            return Result.Fail<Guid>(CustomerErrors.TaxIdInUse);

        var customer = customerResult.Value;

        await repo.CreateAsync(customer, ct);

        return Result.Ok(customer.Id);
    }
}
