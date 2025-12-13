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
        CodigoError codigoError,
        List<string> errores)
        {
            if (string.IsNullOrWhiteSpace(valor))
                errores.Add(ErrorMensaje.Get(codigoError));
        }

        public static void ValidarLongitud(
        string? valor,
        int longitudMaxima,
        CodigoError codigoError,
        List<string> errores)
        {
            if (!string.IsNullOrWhiteSpace(valor) && valor.Length > longitudMaxima)
            {
                errores.Add(string.Format(ErrorMensaje.Get(codigoError), longitudMaxima));
            }
        }
    }
}
