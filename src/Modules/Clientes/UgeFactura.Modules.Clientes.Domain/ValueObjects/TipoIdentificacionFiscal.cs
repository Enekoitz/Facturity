namespace UgeFactura.Modules.Clientes.Domain
{
    public sealed class TipoIdentificacionFiscal
    {
        public string Codigo { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public IdentificacionFiscalReglasEnum Regla { get; init; }
        public bool RequierePais { get; init; } = false;

        // EF Core
        protected TipoIdentificacionFiscal() { }

        public TipoIdentificacionFiscal(string codigo, IdentificacionFiscalReglasEnum regla, bool requierePais)
        {
            Codigo = codigo;
            Regla = regla;
            RequierePais = requierePais;
        }

        public TipoIdentificacionFiscal(string codigo, string nombre, bool requierePais)
        {
            Codigo = codigo;
            Nombre = nombre;
            RequierePais = requierePais;
        }

        public bool EsValido(string valor, out string mensajeError)
        {
            var resultado = IdentificacionFiscalService.Obtener(Regla)(valor);
            mensajeError = resultado.Item2;
            return resultado.Item1;
        }
    }
}
