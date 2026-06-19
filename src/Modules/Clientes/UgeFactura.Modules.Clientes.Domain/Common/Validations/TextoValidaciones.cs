using UgeFactura.Modules.Clientes.Domain;

namespace UgeFactura.Modules.Clientes.Domain
{
    internal static class TextoValidaciones
    {
        internal static void ValidarNoVacio(string? valor,ClienteCodigosError codigoError,List<string> errores)
        {
            if (string.IsNullOrWhiteSpace(valor))
                errores.Add(codigoError.ToString());
        }

        internal static void ValidarLongitud(string? valor,int longitudMaxima,ClienteCodigosError codigoError,List<string> errores)
        {
            if (!string.IsNullOrWhiteSpace(valor) && valor.Length > longitudMaxima)
            {
                errores.Add(codigoError.ToString());
            }
        }
    }
}
