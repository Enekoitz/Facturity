using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgeFactura.Domain.Comun.Validaciones
{
    public static class ValidarCodigos
    {
        public static bool EsCodigoPaisValido(string? codigoPais, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(codigoPais))
            {
                mensajeError = "El código de país no puede estar vacío.";
                return false;
            }

            codigoPais = codigoPais.Trim().ToUpper();
            if (codigoPais.Length != 2)
            {
                mensajeError = "El código de país debe tener una longitud de 2 caracteres.";
                return false;
            }

            return true;
        }
    }
}
