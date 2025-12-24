using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Comun.Mensajes;

namespace UgeFactura.Domain.Comun.Validaciones
{
    public static class ValidarTexto
    {
        public static void ValidarNoVacio(
        string? valor,
        ClienteCodigosError codigoError,
        List<string> errores)
        {
            if (string.IsNullOrWhiteSpace(valor))
                errores.Add(codigoError.ToString());
        }

        public static void ValidarLongitud(
        string? valor,
        int longitudMaxima,
        ClienteCodigosError codigoError,
        List<string> errores)
        {
            if (!string.IsNullOrWhiteSpace(valor) && valor.Length > longitudMaxima)
            {
                errores.Add(codigoError.ToString());
            }
        }
    }
}
