using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed class Customer : AggregateRoot<Guid>
{
    public string Alias { get; private set; }
    public ITaxId TaxId { get; private set; }
    public string? LegalName { get; private set; }
    public string? Name { get; private set; }
    public string? FirstSurname { get; private set; }
    public string? SecondSurname { get; private set; }
    public Address? Address { get; private set; }
    public DateTimeOffset? ActivatedAtUtc { get; private set; }
    public DateTimeOffset? DeactivatedAtUtc { get; private set; }
    public bool IsActive => ActivatedAtUtc is not null && DeactivatedAtUtc is null;
    public DateTimeOffset? DeletedAtUtc { get; private set; }
    
    private Customer() { }

    private Customer(
        Guid id,
        TenantId tenantId,
        string alias,
        ITaxId taxId,
        string? legalName,
        string? name,
        string? firstSurname,
        string? secondSurname,
        Address? address,
        DateTimeOffset activatedAtUtc) : base(id, tenantId)
    {
        Alias = alias;
        TaxId = taxId;
        LegalName = legalName;
        Name = name;
        FirstSurname = firstSurname;
        SecondSurname = secondSurname;
        Address = address;
        ActivatedAtUtc = activatedAtUtc;
    }

    public static Result<Customer> Create(
        Guid id,
        TenantId tenantId,
        string alias,
        ITaxId taxId,
        string? legalName,
        string? name,
        string? firstSurname,
        string? secondSurname,
        Address? address) 
    {

        var errors = new List<Error>();

        if (id == Guid.Empty)
            errors.Add(CustomerErrors.IdInvalid);

        ValidateAlias(alias, errors);

        var tipoDenominacionFiscal = ObtenerTipoDenominacionFiscal(
            taxId.IsLegalEntity, 
            legalName,
            name);

        ValidateLegalNameOrFullName(tipoDenominacionFiscal, legalName, 
            name, firstSurname, secondSurname, errors);

        if (errors.Count > 0)
            return Result.Fail<Customer>(errors);
        
        var activatedAtUtc = DateTimeOffset.UtcNow;

        return Result.Ok(new Customer(id,
                                    tenantId,
                                    alias,
                                    taxId,
                                    legalName,
                                    name,
                                    firstSurname,
                                    secondSurname,
                                    address,
                                    activatedAtUtc));
    }
    
    public static Result<Customer> Rebuild(
        Guid id,
        TenantId tenantId,
        string alias,
        ITaxId taxId,
        string? legalName,
        string? name,
        string? firstSurname,
        string? secondSurname,
        Address? address,
        DateTimeOffset? activatedAtUtc,
        DateTimeOffset? deactivatedAtUtc,
        DateTimeOffset? deletedAtUtc,
        TimeProvider timeProvider) 
    {
        var customerResult = Create(id, tenantId, alias, taxId, legalName, name, firstSurname, secondSurname, address);

        if (customerResult.IsFailure)
            return customerResult;
        
        if (deletedAtUtc is not null && deletedAtUtc > timeProvider.GetUtcNow().UtcDateTime)
            return Result<Customer>.Fail(CustomerErrors.InvalidFutureDate);
        
        var customer = customerResult.Value;
        
        customer.DeletedAtUtc = deletedAtUtc;

        var errores = new List<Error>();
        ValidateActivationOrDeactivation(activatedAtUtc, deactivatedAtUtc, errores, timeProvider);
        
        if(errores.Count > 0)
            return Result.Fail<Customer>(errores);
       
        customer.ActivatedAtUtc =  activatedAtUtc;
        customer.DeactivatedAtUtc = deactivatedAtUtc;
            
        return Result.Ok(customer);
    }

    public Result Update(
        ITaxId? taxId,
        string? legalName,
        string? name,
        string? firstSurname,
        string? secondSurname,
        Address? address)
    {
        var errores = new List<Error>();
        
        var tipoDenominacionFiscal = ObtenerTipoDenominacionFiscal(
            taxId?.IsLegalEntity, 
            legalName,
            name);

        ValidateLegalNameOrFullName(tipoDenominacionFiscal, legalName, 
            name, firstSurname, secondSurname, errores);

        if (errores.Count > 0)
            Result.Fail(errores);

        (LegalName, Name, FirstSurname, SecondSurname) = tipoDenominacionFiscal switch
        {
            TaxDesignationType.LegalName => (legalName, (string?)null, (string?)null, (string?)null),
            TaxDesignationType.FullName => ((string?)null, name, firstSurname, secondSurname),
            _ => (null, null, null, null)
        };
            
        TaxId = taxId ?? TaxId;
        Address = address ?? Address;
        
        return Result.Ok();
    }

    internal static TaxDesignationType? ObtenerTipoDenominacionFiscal(
        bool? isLegalEntity,
        string? legalName,
        string? name)
    {
        return (esPersonaJuridica: isLegalEntity, razonSocial: legalName, nombre: name) switch
        {
            (false, _, _) => TaxDesignationType.FullName,
            (true, _, _)  => TaxDesignationType.LegalName,
    
            var (_, r, _) when !string.IsNullOrWhiteSpace(r) => TaxDesignationType.LegalName,
            var (_, _, n) when !string.IsNullOrWhiteSpace(n) => TaxDesignationType.FullName,
    
            _ => null
        };
    }
    
    private static void ValidateAlias(
        string alias,
        List<Error> errors)
    {
        if (Validate.IsEmpty(alias))
            errors.Add(CustomerErrors.AliasEmpty);

        if (Validate.ExceedsMaximumLength(alias, Parameters.Customer.AliasMaxLength))
            errors.Add(CustomerErrors.AliasTooLong);
    }

    private static void ValidateLegalNameOrFullName(
        TaxDesignationType? taxDesignationType,
        string? legalName,
        string? name,
        string? firstSurname,
        string? secondSurname,
        List<Error> errors
        )
    {
        switch (taxDesignationType, legalName, name)
        {
            case (TaxDesignationType.LegalName, _, _):
                ValidateLegalName(legalName, errors);
                break;

            case (TaxDesignationType.FullName, _, _):
                ValidateFullName(name, firstSurname, secondSurname, errors);
                break;

            // case (null, _, _):
                
            default:
                errors.Add(CustomerErrors.InvalidTaxDesignationType);
                break;
        }
    }

    private static void ValidateFullName(
        string? name, 
        string? firstSurname, 
        string? secondSurname, 
        List<Error> errors)
    {
        if(Validate.IsEmpty(name))
            errors.Add(CustomerErrors.NameEmpty);

        if (Validate.ExceedsMaximumLength(name, Parameters.Customer.NameMaxLength))
            errors.Add(CustomerErrors.NameTooLong);

        if (string.IsNullOrWhiteSpace(firstSurname))
            errors.Add(CustomerErrors.FirstSurnameEmpty);

        if (Validate.ExceedsMaximumLength(firstSurname, Parameters.Customer.SurnameMaxLength))
            errors.Add(CustomerErrors.FirstSurnameTooLong);

        if (!string.IsNullOrWhiteSpace(secondSurname) && Validate.ExceedsMaximumLength(secondSurname, Parameters.Customer.SurnameMaxLength))
            errors.Add(CustomerErrors.SecondSurnameTooLong);
    }

    private static void ValidateLegalName(
        string? legalName,
        List<Error> errors)
    {
        if (Validate.IsEmpty(legalName))
            errors.Add(CustomerErrors.LegalNameEmpty);

        if (Validate.ExceedsMaximumLength(legalName, Parameters.Customer.LegalNameMaxLength))
            errors.Add(CustomerErrors.LegalNameTooLong);
    }

    private static void ValidateActivationOrDeactivation(
        DateTimeOffset? activatedAtUtc, 
        DateTimeOffset? deactivatedAtUtc,
        List<Error> errors,
        TimeProvider timeProvider)
    {
        if (activatedAtUtc is not null && deactivatedAtUtc is not null)
        {
            errors.Add(CustomerErrors.SimultaneousActivationDeactivationDate);
        }
        else if ((activatedAtUtc is not null && activatedAtUtc > timeProvider.GetUtcNow()) 
                 || 
                 (deactivatedAtUtc is not null && deactivatedAtUtc > timeProvider.GetUtcNow()))
        {
            errors.Add(CustomerErrors.InvalidFutureDate);
        }
    }
}