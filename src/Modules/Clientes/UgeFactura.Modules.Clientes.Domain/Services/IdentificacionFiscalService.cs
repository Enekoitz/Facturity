namespace UgeFactura.Modules.Clientes.Domain
{
    public static class IdentificacionFiscalService
    {
        public static Func<string, (bool, string)> Obtener(IdentificacionFiscalReglasEnum regla)
        => regla switch
        {
            IdentificacionFiscalReglasEnum.NifNieCif =>
                valor => (IdentificacionFiscalValidaciones.EsNifNieOCifValido(valor, out var msj), msj),

            IdentificacionFiscalReglasEnum.OtroDocumento =>
                valor => (IdentificacionFiscalValidaciones.EsIdentificacionNoNifValida(valor, out var msj), msj),

            IdentificacionFiscalReglasEnum.NifEntidadExtranjera =>
                valor => (IdentificacionFiscalValidaciones.EsNifEntidadExtranjeraValido(valor, out var msj), msj),

            IdentificacionFiscalReglasEnum.Desconocido => 
                valor => (false, IdentificacionFiscalCodigosError.IdentificacionFiscalReglas_Desconocido.ToString()),
            
            _ =>
                valor => (false, String.Empty)
        };

        public static IdentificacionFiscalReglasEnum ParseOrDesconocido(string value)
        {
            if (Enum.TryParse<IdentificacionFiscalReglasEnum>(value, out var result))
                return result;

            return IdentificacionFiscalReglasEnum.Desconocido;
        }
    }
}
