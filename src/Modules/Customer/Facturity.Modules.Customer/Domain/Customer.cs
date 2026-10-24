using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed class Customer : AggregateRoot<Guid>
{
    public string Alias { get; private set; }
    public ITaxId TaxId { get; private set; }
    public string? RazonSocial { get; private set; }
    public string? Nombre { get; private set; }
    public string? PrimerApellido { get; private set; }
    public string? SegundoApellido { get; private set; }
    public Address? Domicilio { get; private set; }
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
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Address? domicilio,
        DateTimeOffset activatedAtUtc) : base(id, tenantId)
    {
        Alias = alias;
        TaxId = taxId;
        RazonSocial = razonSocial;
        Nombre = nombre;
        PrimerApellido = primerApellido;
        SegundoApellido = segundoApellido;
        Domicilio = domicilio;
        ActivatedAtUtc = activatedAtUtc;
    }

    public static Result<Customer> Crear(
        Guid id,
        TenantId tenantId,
        string alias,
        ITaxId taxId,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Address? domicilio) 
    {

        var errores = new List<Error>();

        if (id == Guid.Empty)
            errores.Add(CustomerErrors.IdInvalid);

        ValidarAlias(alias, errores);

        var tipoDenominacionFiscal = ObtenerTipoDenominacionFiscal(
            taxId.IsLegalEntity, 
            razonSocial,
            nombre);

        ValidarNombresApellidosRazonSocial(tipoDenominacionFiscal, razonSocial, 
            nombre, primerApellido, segundoApellido, errores);

        if (errores.Count > 0)
            return Result.Fail<Customer>(errores);
        
        var activatedAtUtc = DateTimeOffset.UtcNow;

        return Result.Ok(new Customer(id,
                                    tenantId,
                                    alias,
                                    taxId,
                                    razonSocial,
                                    nombre,
                                    primerApellido,
                                    segundoApellido,
                                    domicilio,
                                    activatedAtUtc));
    }
    
    public static Result<Customer> Reconstruir(
        Guid id,
        TenantId tenantId,
        string alias,
        ITaxId taxId,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Address? domicilio,
        DateTimeOffset? activatedAtUtc,
        DateTimeOffset? deactivatedAtUtc,
        DateTimeOffset? deletedAtUtc,
        TimeProvider timeProvider) 
    {
        var clienteResult = Crear(id, tenantId, alias, taxId, razonSocial, nombre, primerApellido, segundoApellido, domicilio);

        if (clienteResult.IsFailure)
            return clienteResult;
        
        if (deletedAtUtc is not null && deletedAtUtc > timeProvider.GetUtcNow().UtcDateTime)
            return Result<Customer>.Fail(CustomerErrors.InvalidFutureDate);
        
        var cliente = clienteResult.Value;
        
        cliente.DeletedAtUtc = deletedAtUtc;

        var errores = new List<Error>();
        ValidarActivacionDesactivacion(activatedAtUtc, deactivatedAtUtc, errores, timeProvider);
        
        if(errores.Count > 0)
            return Result.Fail<Customer>(errores);
       
        cliente.ActivatedAtUtc =  activatedAtUtc;
        cliente.DeactivatedAtUtc = deactivatedAtUtc;
            
        return Result.Ok(cliente);
    }

    public Result Actualizar(
        ITaxId? identificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Address? domicilio)
    {
        var errores = new List<Error>();
        
        var tipoDenominacionFiscal = ObtenerTipoDenominacionFiscal(
            identificacionFiscal.IsLegalEntity, 
            razonSocial,
            nombre);

        ValidarNombresApellidosRazonSocial(tipoDenominacionFiscal, razonSocial, 
            nombre, primerApellido, segundoApellido, errores);

        if (errores.Count > 0)
            Result.Fail(errores);

        (RazonSocial, Nombre, PrimerApellido, SegundoApellido) = tipoDenominacionFiscal switch
        {
            TaxDesignationType.LegalName => (razonSocial, (string?)null, (string?)null, (string?)null),
            TaxDesignationType.FirsLastName => ((string?)null, nombre, primerApellido, segundoApellido),
            _ => (null, null, null, null)
        };
            
        TaxId = identificacionFiscal;
        Domicilio = domicilio;
        
        return Result.Ok();
    }

    private static void ValidarAlias(
        string alias,
        List<Error> errores)
    {
        if (Validate.EsVacio(alias))
            errores.Add(CustomerErrors.AliasEmpty);

        if (Validate.SuperaLongitudMaxima(alias, Parameters.Customer.AliasMaxLength))
            errores.Add(CustomerErrors.AliasTooLong);
    }

    internal static TaxDesignationType? ObtenerTipoDenominacionFiscal(
        bool? esPersonaJuridica,
        string? razonSocial,
        string? nombre)
    {
        return (esPersonaJuridica, razonSocial, nombre) switch
        {
            (false, _, _) => TaxDesignationType.FirsLastName,
            (true, _, _)  => TaxDesignationType.LegalName,
    
            var (_, r, _) when !string.IsNullOrWhiteSpace(r) => TaxDesignationType.LegalName,
            var (_, _, n) when !string.IsNullOrWhiteSpace(n) => TaxDesignationType.FirsLastName,
    
            _ => null
        };
    }

    private static void ValidarNombresApellidosRazonSocial(
        TaxDesignationType? tipoDenominacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        List<Error> errores
        )
    {
        switch (tipoDenominacionFiscal, razonSocial, nombre)
        {
            case (TaxDesignationType.LegalName, _, _):
                ValidarRazonSocial(razonSocial, errores);
                break;

            case (TaxDesignationType.FirsLastName, _, _):
                ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                break;

            case (null, _, _):
            default:
                errores.Add(CustomerErrors.InvalidTaxDesignationType);
                break;
        }
    }

    private static void ValidarNombreApellido(
        string? nombre, 
        string? primerApellido, 
        string? segundoApellido, 
        List<Error> errores)
    {
        if(Validate.EsVacio(nombre))
            errores.Add(CustomerErrors.NameEmpty);

        if (Validate.SuperaLongitudMaxima(nombre, Parameters.Customer.NameMaxLength))
            errores.Add(CustomerErrors.NameTooLong);

        if (string.IsNullOrWhiteSpace(primerApellido))
            errores.Add(CustomerErrors.FirstSurnameEmpty);

        if (Validate.SuperaLongitudMaxima(primerApellido, Parameters.Customer.SurnameMaxLength))
            errores.Add(CustomerErrors.FirstSurnameTooLong);

        if (!string.IsNullOrWhiteSpace(segundoApellido) && Validate.SuperaLongitudMaxima(segundoApellido, Parameters.Customer.SurnameMaxLength))
            errores.Add(CustomerErrors.SecondSurnameTooLong);
    }

    private static void ValidarRazonSocial(
        string? razonSocial,
        List<Error> errores)
    {
        if (Validate.EsVacio(razonSocial))
            errores.Add(CustomerErrors.LegalNameEmpty);

        if (Validate.SuperaLongitudMaxima(razonSocial, Parameters.Customer.LegalNameMaxLength))
            errores.Add(CustomerErrors.LegalNameTooLong);
    }

    private static void ValidarActivacionDesactivacion(
        DateTimeOffset? activatedAtUtc, 
        DateTimeOffset? deactivatedAtUtc,
        List<Error> errores,
        TimeProvider timeProvider)
    {
        if (activatedAtUtc is not null && deactivatedAtUtc is not null)
        {
            errores.Add(CustomerErrors.SimultaneousActivationDeactivationDate);
        }
        else if ((activatedAtUtc is not null && activatedAtUtc > timeProvider.GetUtcNow().UtcDateTime) 
                 || 
                 (deactivatedAtUtc is not null && deactivatedAtUtc > timeProvider.GetUtcNow().UtcDateTime))
        {
            errores.Add(CustomerErrors.InvalidFutureDate);
        }
    }
}