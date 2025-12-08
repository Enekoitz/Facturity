using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgeFactura.Domain.Cliente.ValueObjects
{
    public sealed record SubTipoIdentificacion
    {
        public string Codigo { get; }
        public string Descripcion { get; }

        private SubTipoIdentificacion(string codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }

        public static readonly SubTipoIdentificacion NifPersonaFisica =
            new("NIF_PF", "NIF Persona Física");

        public static readonly SubTipoIdentificacion Nie =
            new("NIE", "Número de Identidad de Extranjero");

        public static readonly SubTipoIdentificacion Cif =
            new("CIF", "Código de Identificación Fiscal - Persona Jurídica");

        public static readonly SubTipoIdentificacion NifExtranjero =
            new("NIF_EXT", "NIF Extranjero");

        public override string ToString() => Descripcion;
    }
}
