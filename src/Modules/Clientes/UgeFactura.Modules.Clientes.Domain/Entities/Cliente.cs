using UgeFactura.Modules.DomainSharedKernel;

namespace UgeFactura.Modules.Clientes.Domain;

public class Cliente : Entity
{
    public string Alias { get; private set; }
    public IdentificacionFiscal Identificacion { get; private set; }
    public string? RazonSocial { get; private set; }
    public string? Nombre { get; private set; }
    public string? PrimerApellido { get; private set; }
    public string? SegundoApellido { get; private set; }
    public Domicilio? Domicilio { get; private set; }

    protected Cliente() { }

    private Cliente(
        Guid id,
        string alias,
        IdentificacionFiscal identificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Domicilio? domicilio) : base(id)
    {
        Alias = alias;
        Identificacion = identificacionFiscal;
        RazonSocial = razonSocial;
        Nombre = nombre;
        PrimerApellido = primerApellido;
        SegundoApellido = segundoApellido;
        Domicilio = domicilio;
    }

    public static Result<Cliente> Crear(
        Guid id,
        string alias,
        IdentificacionFiscal identificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Domicilio? domicilio) 
    {

        var errores = new List<Error>();

        if(id == Guid.Empty)
            errores.Add(Error.Validation(ClienteCodigosError.Id_Vacio.ToString(), "El identificador del cliente no es válido."));

        ValidarAlias(alias, errores);

        SubtiposIdentificacionFiscalReglas? subtipoIdentificacionFiscal = SubtipoIdentificacionFiscalService.ObtenerSubtipo(identificacionFiscal.Tipo.Regla, identificacionFiscal.Valor);

        ValidarNombresApellidosRazonSocial(subtipoIdentificacionFiscal, razonSocial, nombre, primerApellido, segundoApellido, errores);

        if (errores.Count > 0)
            return Result.Fail<Cliente>(errores);

        return Result.Ok(new Cliente(id,
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
        if (Validar.EsVacio(alias))
            errores.Add(Error.Validation(ClienteCodigosError.Alias_Vacio.ToString(), "El alias no puede ser vacío."));

        if (Validar.SuperaLongitudMaxima(alias, Parametros.AliasLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.Alias_DemasiadoLargo.ToString(), 
                $"El alias no puede superar los {Parametros.AliasLongitudMaxima} caracteres."));
    }

    private static void ValidarNombresApellidosRazonSocial(
        SubtiposIdentificacionFiscalReglas? subtipoIdentificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        List<Error> errores)
    {
        switch (subtipoIdentificacionFiscal)
        {
            case SubtiposIdentificacionFiscalReglas.DNI:
            case SubtiposIdentificacionFiscalReglas.Nie:
                ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                break;

            case SubtiposIdentificacionFiscalReglas.Cif:
                ValidarRazonSocial(razonSocial, errores);
                break;

            default:
                if (!string.IsNullOrWhiteSpace(razonSocial))
                    ValidarRazonSocial(razonSocial, errores);
                else
                    ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                break;
        }
    }

    private static void ValidarNombreApellido(
        string? nombre, 
        string? primerApellido, 
        string? segundoApellido, 
        List<Error> errores)
    {

        if(Validar.EsVacio(nombre))
            errores.Add(Error.Validation(ClienteCodigosError.Nombre_Vacio.ToString(), "El nombre no puede ser vacío."));

        if (Validar.SuperaLongitudMaxima(nombre, Parametros.NombreLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.Nombre_DemasiadoLargo.ToString(),
                $"El nombre no puede superar los {Parametros.NombreLongitudMaxima} caracteres."));

        if (Validar.EsVacio(primerApellido))
            errores.Add(Error.Validation(ClienteCodigosError.PrimerApellido_Vacio.ToString(), "El primer apellido no puede ser vacío."));

        if (Validar.SuperaLongitudMaxima(primerApellido, Parametros.ApellidoLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.PrimerApellido_DemasiadoLargo.ToString(),
                $"El primer apellido no puede superar los {Parametros.ApellidoLongitudMaxima} caracteres."));

        if (Validar.SuperaLongitudMaxima(segundoApellido, Parametros.ApellidoLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.SegundoApellido_DemasiadoLargo.ToString(),
                $"El segundo apellido no puede superar los {Parametros.ApellidoLongitudMaxima} caracteres."));
    }

    private static void ValidarRazonSocial(
        string? razonSocial,
        List<Error> errores)
    {
        if (Validar.EsVacio(razonSocial))
            errores.Add(Error.Validation(ClienteCodigosError.RazonSocial_Vacio.ToString(), "La razon social no puede ser vacío."));

        if (Validar.SuperaLongitudMaxima(razonSocial, Parametros.RazonSocialLongitudMaxima))
            errores.Add(Error.Validation(
                ClienteCodigosError.RazonSocial_DemasiadoLargo.ToString(),
                $"La razon social no puede superar los {Parametros.RazonSocialLongitudMaxima} caracteres."));
    }
}