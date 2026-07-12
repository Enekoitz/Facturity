namespace UgeFactura.Modules.Clientes.Domain;

internal static class CodigosValidaciones
{
    internal static bool EsCodigoPaisValido(string? codigoPais, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(codigoPais))
        {
            codigoError = IdentificacionFiscalCodigosError.CodigoPais_Vacio.ToString();
            descripcionError = "El código de pais no puede estar vacío.";
            return false;
        }

        codigoPais = codigoPais.Trim().ToUpper();
        if (codigoPais.Length != 2)
        {
            codigoError = IdentificacionFiscalCodigosError.CodigoPais_LongitudInvalida.ToString();
            descripcionError = "";
            return false;
        }

        return true;
    }
}