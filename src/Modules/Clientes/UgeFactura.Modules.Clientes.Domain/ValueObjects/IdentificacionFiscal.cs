using UgeFactura.Modules.DomainSharedKernel;

namespace UgeFactura.Modules.Clientes.Domain;

public sealed class IdentificacionFiscal : ValueObject
{
    public TipoIdentificacionFiscal Tipo { get; private set; }
    public string Valor { get; private set; }
    public string? CodigoPais { get; private set; }

    protected IdentificacionFiscal() { }

    private IdentificacionFiscal(TipoIdentificacionFiscal tipo, string valor, string? codigoPais)
    {
        Tipo = tipo;
        Valor = valor;
        CodigoPais = codigoPais;
    }

    public static Result<IdentificacionFiscal> Crear(
        string tipoCodigo,
        IdentificacionFiscalReglasEnum tipoRegla,
        string valor,
        string? codigoPais = null)
    {
        tipoCodigo = tipoCodigo?.Trim().ToUpper() ?? string.Empty;
        valor = valor?.Trim().ToUpper() ?? string.Empty;

        codigoPais = string.IsNullOrWhiteSpace(codigoPais) ? null : codigoPais.Trim().ToUpper();

        var errores = new List<Error>();
        
        if (string.IsNullOrWhiteSpace(tipoCodigo))
            errores.Add(Error.Validation(IdentificacionFiscalCodigosError.TipoIdentificacionFiscal_Vacio.ToString(), 
                "El código de tipo de identificación no puede estar vacío."));

        if (string.IsNullOrWhiteSpace(valor))
            errores.Add(Error.Validation(IdentificacionFiscalCodigosError.ValorIdentificacionFiscal_Vacio.ToString(), 
                "El valor de la identificación no puede estar vacío."));

        bool requierePais = codigoPais is not null;
        var tipoIdentificacionFiscal = new TipoIdentificacionFiscal(tipoCodigo, tipoRegla, requierePais);

        if (tipoIdentificacionFiscal.RequierePais)
        {
            if (codigoPais is null)
            {
                errores.Add(Error.Validation(IdentificacionFiscalCodigosError.CodigoPais_Vacio.ToString(), 
                    "El código de país no puede estar vacío."));
            }
            else if (codigoPais.Length != Parametros.CodigoPais)
            {
                errores.Add(Error.Validation(IdentificacionFiscalCodigosError.CodigoPais_LongitudInvalida.ToString(), 
                    $"El código de país debe tener exactamente {Parametros.CodigoPais} caracteres."));
            }
        }

        if (!tipoIdentificacionFiscal.EsValido(valor, out string codigoError, out string descripcionError))
            errores.Add(Error.Validation(codigoError, descripcionError));

        if (errores.Count > 0)
            return Result.Fail<IdentificacionFiscal>(errores);

        return Result.Ok(new IdentificacionFiscal(tipoIdentificacionFiscal, valor, codigoPais));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Tipo;
        yield return Valor;
        yield return CodigoPais;
    }
}
