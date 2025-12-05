using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgeFactura.Domain.Entidades
{
    internal class Cliente
    {
        private Guid Guid { get; set; }
        private string NIF { get; set; }
        private string RazonSocial { get; set; }
        private string Nombre { get; set; }
        private string ApellidoPrimero { get; set; }
        private string ApellidoSegundo { get; set; }
        private string Direccion { get; set; }  
        private string CodigoPostal { get; set; }
    }
}
