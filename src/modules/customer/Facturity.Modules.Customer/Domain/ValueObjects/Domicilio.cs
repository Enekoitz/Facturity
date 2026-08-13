using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record Domicilio
{
    public string Direccion { get; private set; }
    public CodigoPostal? CodigoPostal { get; private set; }
    public string? Municipio { get; private set; }
    public string? Provincia { get; private set; }
    public string? Region { get; private set; }
    public string? Pais { get; private set; }

    protected Domicilio() { }

    private Domicilio(
        string direccion
        , CodigoPostal? codigoPostal
        , string? municipio
        , string? provincia
        , string? region
        , string? pais)
    {
        Direccion = direccion;
        CodigoPostal = codigoPostal;
        Municipio = municipio;
        Provincia = provincia;
        Region = region;
        Pais = pais;
    }

    public static Result<Domicilio> Crear(
        string direccion
        , string? codigoPostal
        , string? municipio                       
        , string? provincia
        , string? region
        , string? pais)
    {
        var errores = new List<Error>();

        if (string.IsNullOrWhiteSpace(direccion))
            errores.Add(Error.Validation(nameof(DomicilioCodigosError.DireccionVacio), "La dirección no puede ser vacía."));

        municipio = string.IsNullOrWhiteSpace(municipio) ? null : municipio.Trim();
        provincia = string.IsNullOrWhiteSpace(provincia) ? null : provincia.Trim();
        region = string.IsNullOrWhiteSpace(region) ? null : region.Trim();
        pais = string.IsNullOrWhiteSpace(pais) ? null : pais.Trim();

        CodigoPostal? codigoPostalVo = null;
        if (!string.IsNullOrWhiteSpace(codigoPostal))
        {
            var codigoPostalResult = CodigoPostal.Crear(codigoPostal);
            if (codigoPostalResult.IsFailure)
                errores.AddRange(codigoPostalResult.Errors);
            else
                codigoPostalVo = codigoPostalResult.Value;
        }

        if (errores.Count > 0)
            return Result.Fail<Domicilio>(errores);

        direccion = direccion.Trim();

        return Result.Ok(new Domicilio(
            direccion,
            codigoPostalVo, 
            municipio, 
            provincia, 
            region, 
            pais));
    }
}