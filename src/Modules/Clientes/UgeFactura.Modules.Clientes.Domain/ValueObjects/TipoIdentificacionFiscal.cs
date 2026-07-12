namespace UgeFactura.Modules.Clientes.Domain;

public sealed class TipoIdentificacionFiscal
{
    public string Codigo { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public IdentificacionFiscalReglasEnum Regla { get; init; }
    public bool RequierePais { get; init; } = false;

    protected TipoIdentificacionFiscal() { }

    public TipoIdentificacionFiscal(string codigo, IdentificacionFiscalReglasEnum regla, bool requierePais)
    {
        Codigo = codigo;
        Regla = regla;
        RequierePais = requierePais;
    }

    public TipoIdentificacionFiscal(string codigo, string nombre, IdentificacionFiscalReglasEnum regla, bool requierePais)
    {
        Codigo = codigo;
        Nombre = nombre;
        Regla = regla;
        RequierePais = requierePais;
    }

    public bool EsValido(string valor, out string codigoError, out string descripcionError)
    {
        var (esValido, codigoErrorOTPUT, descripcionErrorOTPUT) = IdentificacionFiscalService.Obtener(Regla)(valor);
        codigoError = codigoErrorOTPUT;
        descripcionError = descripcionErrorOTPUT;

        return esValido;
    }
}
