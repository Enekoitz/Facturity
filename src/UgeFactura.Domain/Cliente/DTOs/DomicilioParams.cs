using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.ValueObjects;

namespace UgeFactura.Domain.Cliente.DTOs
{
    public class DomicilioParams
    {
        public string Direccion { get; set; } = String.Empty;
        public string CP { get; set; } = String.Empty;
        public string Municipio { get; set; } = String.Empty;
        public string Provincia { get; set; } = String.Empty;
        public string Region { get; set; } = String.Empty;
        public string Pais { get; set; } = String.Empty;
    }
}
