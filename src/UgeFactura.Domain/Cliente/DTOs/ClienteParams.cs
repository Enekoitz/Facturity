using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.ValueObjects;

namespace UgeFactura.Domain.Cliente.DTOs
{
    public class ClienteParams
    {
        public string Alias { get; set; } = string.Empty;
        public IdentificacionParams Identificacion { get; set; } = new IdentificacionParams();
        public string? Nombre { get; set; }
        public string? RazonSocial { get; set; } 
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public DomicilioParams Domicilio { get; set; } = new DomicilioParams();
    }
}
