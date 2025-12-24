using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun.Mensajes;

namespace UgeFactura.Domain.Comun.Validaciones
{
    public static class ValidarCodigos
    {
        public static bool EsCodigoPaisValido(string? codigoPais, out string codigoError)
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
