using System;
using System.Collections.Generic;
using System.Text;

namespace UgeFactura.Application.Clientes
{
    public sealed class CrearClienteCommand
    {
        public string Alias { get; }
        public string IdentificacionTipoCodigo { get; } 
        public string IdentificacionValor { get; }
        public string? IdentificacionPais { get; }
        public string? RazonSocial { get; } 
        public string? Nombre { get; } 
        public string? PrimerApellido { get; }
        public string? SegundoApellido { get; }
        public string Direccion { get; } 
        public string? CodigoPostal { get; }
        public string? Municipio { get; } 
        public string? Provincia { get; } 
        public string? Region { get; }
        public string? Pais { get; } 

        public CrearClienteCommand(string alias, string identificacionTipoCodigo, string identificacionValor, string identificacionPais, string? razonSocial, string? nombre, string? primerApellido, string? segundoApellido, string direccion, string? codigoPostal, string? municipio, string? provincia, string? region, string? pais)
        {
            Alias  = alias;
            IdentificacionTipoCodigo  = identificacionTipoCodigo;
            IdentificacionValor  = identificacionValor;
            IdentificacionPais  = identificacionPais;
            RazonSocial  = razonSocial;
            Nombre  = nombre;
            PrimerApellido  = primerApellido;
            SegundoApellido  = segundoApellido;
            Direccion  = direccion;
            CodigoPostal  = codigoPostal;
            Municipio  = municipio;
            Provincia  = provincia;
            Region  = region;
            Pais  = pais;
        }
    }
}
