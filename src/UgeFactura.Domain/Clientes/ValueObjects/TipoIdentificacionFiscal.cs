using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgeFactura.Domain.Clientes
{
    public sealed class  TipoIdentificacionFiscal
    {
        public string Codigo { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public ReglaIdentificacionFiscal Regla { get; init; }
        public bool RequierePais { get; init; } = false;

        protected TipoIdentificacionFiscal() { }

        public TipoIdentificacionFiscal(string codigo, ReglaIdentificacionFiscal regla, bool requierePais)
        {
            Codigo = codigo;
            Regla = regla;
            RequierePais = requierePais;
        }

        public bool EsValido(string valor, out string mensajeError)
        {
            var resultado = IdentificacionFiscalReglas.Obtener(Regla)(valor);
            mensajeError = resultado.Item2;
            return resultado.Item1;
        }
    }
}
