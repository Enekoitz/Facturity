using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed class Cliente : AggregateRoot<Guid>
{
    public string Alias { get; private set; }
    public IIdentificacionFiscal IdentificacionFiscal { get; private set; }
    public string? RazonSocial { get; private set; }
    public string? Nombre { get; private set; }
    public string? PrimerApellido { get; private set; }
    public string? SegundoApellido { get; private set; }
    public Domicilio? Domicilio { get; private set; }

    private Cliente() { }

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
            errores.Add(Error.Validation(nameof(ClienteCodigosError.IdVacio), "El identificador del cliente no es válido."));

        if (tenantId.Value <= 0)
            errores.Add(Error.Validation(nameof(TenantCodigos.TenantIdInvalido), "El TenantId no es válido."));

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
            errores.Add(Error.Validation(nameof(ClienteCodigosError.AliasVacio), "El alias no puede ser vacío."));

        if (Validate.SuperaLongitudMaxima(alias, ClienteParametros.AliasMaxLength))
            errores.Add(Error.Validation(
                nameof(ClienteCodigosError.AliasDemasiadoLargo), 
                $"El alias no puede superar los {ClienteParametros.AliasMaxLength} caracteres."));
    }

    private static void ValidarNombresApellidosRazonSocial(
        bool? esPersonaJuridica,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        List<Error> errores)
    {
        switch (esPersonaJuridica)
        {
            case false:
                ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                break;
            case true:
                ValidarRazonSocial(razonSocial, errores);
                break;
            default:
            {
                if (!string.IsNullOrWhiteSpace(razonSocial))
                {
                    ValidarRazonSocial(razonSocial, errores);
                }
                else
                {
                    ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                }

                break;
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
                nameof(ClienteCodigosError.NombreVacio), 
                "El nombre no puede ser vacío."));

        if (Validate.SuperaLongitudMaxima(nombre, ClienteParametros.NombreMaxLength))
            errores.Add(Error.Validation(
                nameof(ClienteCodigosError.NombreDemasiadoLargo),
                $"El nombre no puede superar los {ClienteParametros.NombreMaxLength} caracteres."));

        if (Validate.EsVacio(primerApellido))
            errores.Add(Error.Validation(
                nameof(ClienteCodigosError.PrimerApellidoVacio), 
                "El primer apellido no puede ser vacío."));

        if (Validate.SuperaLongitudMaxima(primerApellido, ClienteParametros.ApellidoMaxLength))
            errores.Add(Error.Validation(
                nameof(ClienteCodigosError.PrimerApellidoDemasiadoLargo),
                $"El primer apellido no puede superar los {ClienteParametros.ApellidoMaxLength} caracteres."));

        if (Validate.SuperaLongitudMaxima(segundoApellido, ClienteParametros.ApellidoMaxLength))
            errores.Add(Error.Validation(
                nameof(ClienteCodigosError.SegundoApellidoDemasiadoLargo),
                $"El segundo apellido no puede superar los {ClienteParametros.ApellidoMaxLength} caracteres."));
    }

    private static void ValidarRazonSocial(
        string? razonSocial,
        List<Error> errores)
    {
        if (Validate.EsVacio(razonSocial))
            errores.Add(Error.Validation(
                nameof(ClienteCodigosError.RazonSocialVacio), 
                "La razon social no puede ser vacío."));

        if (Validate.SuperaLongitudMaxima(razonSocial, ClienteParametros.RazonSocialMaxLength))
            errores.Add(Error.Validation(
                nameof(ClienteCodigosError.RazonSocialDemasiadoLargo),
                $"La razon social no puede superar los {ClienteParametros.RazonSocialMaxLength} caracteres."));
    }
}