
namespace UgeFactura.Modules.Clientes.Domain
{
    internal static class CodesValidations
    {
        internal static bool EsCodigoPaisValido(string? codigoPais, out string codigoError)
        {
            codigoError = string.Empty;

            if (string.IsNullOrWhiteSpace(codigoPais))
            {
                codigoError = IdentificacionFiscalCodigosError.CodigoPais_Vacio.ToString();
                return false;
            }

            codigoPais = codigoPais.Trim().ToUpper();
            if (codigoPais.Length != 2)
            {
                codigoError = IdentificacionFiscalCodigosError.CodigoPais_LongitudInvalida.ToString();
                return false;
            }

            return true;
        }
    }
}