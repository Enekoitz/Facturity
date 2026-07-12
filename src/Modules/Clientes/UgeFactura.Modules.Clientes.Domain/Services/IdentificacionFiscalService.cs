namespace UgeFactura.Modules.Clientes.Domain
{
    public static class IdentificacionFiscalService
    {
        public static Func<string, (bool esValido, string codigoError, string descripcionError)> Obtener(IdentificacionFiscalReglasEnum regla)
        => regla switch
        {
            IdentificacionFiscalReglasEnum.Nif =>
                valor => (IdentificacionFiscalValidaciones.EsNifValido(valor, out string codigoError, out string descripcionError), 
                        codigoError,
                        descripcionError),

            IdentificacionFiscalReglasEnum.OtroDocumento =>
                valor => (IdentificacionFiscalValidaciones.EsIdentificacionNoNifValida(valor, out string codigoError, out string descripcionError),
                        codigoError,
                        descripcionError),

            IdentificacionFiscalReglasEnum.NifEntidadExtranjera =>
                valor => (IdentificacionFiscalValidaciones.EsNifEntidadExtranjeraValido(valor, out string codigoError, out string descripcionError),
                        codigoError,
                        descripcionError),

            IdentificacionFiscalReglasEnum.Desconocido => 
                valor => (false, 
                        IdentificacionFiscalCodigosError.IdentificacionFiscalReglas_Desconocido.ToString(),
                        "El tipo de identificacion fiscal es desconocido"),
            
            _ =>
                valor => (false, string.Empty, string.Empty)
        };

        public static IdentificacionFiscalReglasEnum ParseOrDesconocido(string value)
        {
            if (Enum.TryParse<IdentificacionFiscalReglasEnum>(value, out var result))
                return result;

            return IdentificacionFiscalReglasEnum.Desconocido;
        }
    }
}
