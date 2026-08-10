using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public class Cliente : AggregateRoot<Guid>
{
    public string Alias { get; private set; }
    public IIdentificacionFiscal IdentificacionFiscal { get; private set; }
    public string? RazonSocial { get; private set; }
    public string? Nombre { get; private set; }
    public string? PrimerApellido { get; private set; }
    public string? SegundoApellido { get; private set; }
    public Domicilio? Domicilio { get; private set; }

    protected Cliente() { }

    private Cliente(
        Guid id,
        TenantId tenantId,
        string alias,
        IIdentificacionFiscal identificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Domicilio? domicilio) : base(id, tenantId)
    {
        Alias = alias;
        IdentificacionFiscal = identificacionFiscal;
        RazonSocial = razonSocial;
        Nombre = nombre;
        PrimerApellido = primerApellido;
        SegundoApellido = segundoApellido;
        Domicilio = domicilio;
    }

    //public TiposIdentificacionFiscal ObtenerTipoIdentificacionFiscal() => IdentificacionFiscal switch
    //{
    //    NifEspañol => TiposIdentificacionFiscal.NifEspañol,
    //    IdOtroExtranjero => TiposIdentificacionFiscal.IdOtroExtranjero,
    //    null => throw new ArgumentNullException(nameof(IdentificacionFiscal), "La identificación fiscal no puede ser nula."),
    //    _ => throw new InvalidOperationException($"El tipo de identificación fiscal '{IdentificacionFiscal.GetType().Name}' no es soportado.")
    //};

    public static Result<Cliente> Crear(
        Guid id,
        TenantId tenantId,
        string alias,
        IIdentificacionFiscal identificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Domicilio? domicilio) 
    {

        var errores = new List<Error>();

        if (id == Guid.Empty)
            errores.Add(Error.Validation(ClienteCodigosError.Id_Vacio.ToString(), "El identificador del cliente no es válido."));

        if (tenantId.Value <= 0)
            errores.Add(Error.Validation("TenantIdInvalido", "El TenantId no es válido."));

        ValidarAlias(alias, errores);

        ValidarNombresApellidosRazonSocial(identificacionFiscal.EsPersonaJuridica, razonSocial, nombre, primerApellido, segundoApellido, errores);

        if (errores.Count > 0)
            return Result.Fail<Cliente>(errores);

        return Result.Ok(new Cliente(id,
                                    tenantId,
                                    alias,
                                    identificacionFiscal,
                                    razonSocial,
                                    nombre,
                                    primerApellido,
                                    segundoApellido,
                                    domicilio));
    }

    private static void ValidarAlias(
        string alias,
        List<Error> errores)
    {
        if (Validate.EsVacio(alias))
            errores.Add(Error.Validation(ClienteCodigosError.Alias_Vacio.ToString(), "El alias no puede ser vacío."));

        if (Validate.SuperaLongitudMaxima(alias, Parametros.AliasLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.Alias_DemasiadoLargo.ToString(), 
                $"El alias no puede superar los {Parametros.AliasLongitudMaxima} caracteres."));
    }

    private static void ValidarNombresApellidosRazonSocial(
        bool? esPersonaJuridica,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        List<Error> errores)
    {
        if (esPersonaJuridica == true)
        {
            ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
        }
        else if (esPersonaJuridica == false)
        {
            ValidarRazonSocial(razonSocial, errores);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(razonSocial))
            {
                ValidarRazonSocial(razonSocial, errores);
            }
            else
            {
                ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
            }
        }
    }

    private static void ValidarNombreApellido(
        string? nombre, 
        string? primerApellido, 
        string? segundoApellido, 
        List<Error> errores)
    {

        if(Validate.EsVacio(nombre))
            errores.Add(Error.Validation(
                ClienteCodigosError.Nombre_Vacio.ToString(), 
                "El nombre no puede ser vacío."));

        if (Validate.SuperaLongitudMaxima(nombre, Parametros.NombreLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.Nombre_DemasiadoLargo.ToString(),
                $"El nombre no puede superar los {Parametros.NombreLongitudMaxima} caracteres."));

        if (Validate.EsVacio(primerApellido))
            errores.Add(Error.Validation(
                ClienteCodigosError.PrimerApellido_Vacio.ToString(), 
                "El primer apellido no puede ser vacío."));

        if (Validate.SuperaLongitudMaxima(primerApellido, Parametros.ApellidoLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.PrimerApellido_DemasiadoLargo.ToString(),
                $"El primer apellido no puede superar los {Parametros.ApellidoLongitudMaxima} caracteres."));

        if (Validate.SuperaLongitudMaxima(segundoApellido, Parametros.ApellidoLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.SegundoApellido_DemasiadoLargo.ToString(),
                $"El segundo apellido no puede superar los {Parametros.ApellidoLongitudMaxima} caracteres."));
    }

    private static void ValidarRazonSocial(
        string? razonSocial,
        List<Error> errores)
    {
        if (Validate.EsVacio(razonSocial))
            errores.Add(Error.Validation(
                ClienteCodigosError.RazonSocial_Vacio.ToString(), 
                "La razon social no puede ser vacío."));

        if (Validate.SuperaLongitudMaxima(razonSocial, Parametros.RazonSocialLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.RazonSocial_DemasiadoLargo.ToString(),
                $"La razon social no puede superar los {Parametros.RazonSocialLongitudMaxima} caracteres."));
    }
}