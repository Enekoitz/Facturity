using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.ValueObjects;

namespace UgeFactura.Domain.Cliente.DTOs
{
    public class IdentificacionParams
    {
        public string CodigoTipo { get; set; } = String.Empty;
        public string Valor { get; set; } = String.Empty;
        public string? PaisCodigo { get; set; }
    }
}
