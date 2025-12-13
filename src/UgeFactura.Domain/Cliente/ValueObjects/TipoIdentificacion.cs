using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgeFactura.Domain.Cliente.ValueObjects
{
    public sealed record TipoIdentificacionFiscal
    {
        public string Codigo { get; init; }
        public string Tipo { get; init; }
        public bool RequierePais { get; init; } = false;
        private Func<string, (bool, string)> Validador { get; init; }

        public TipoIdentificacionFiscal(string codigo, string tipo, Func<string, (bool, string)> validador, bool requierePais) { 
            Codigo = codigo;
            Tipo = tipo;
            Validador = validador;
            RequierePais = requierePais;
        }

        public bool EsValido(string valor, out string mensajeError)
        {
            (bool esValido, mensajeError) = Validador(valor);
            return esValido;
        }
    }
}
