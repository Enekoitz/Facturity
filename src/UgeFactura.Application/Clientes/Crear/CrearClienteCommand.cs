using System;
using System.Collections.Generic;
using System.Text;

namespace UgeFactura.Application.Clientes
{
    public sealed class CrearClienteCommand(string alias, string identificacionTipoCodigo, string identificacionValor, string identificacionPais, string? razonSocial, string? nombre, string? primerApellido, string? segundoApellido, string direccion, string? codigoPostal, string? municipio, string? provincia, string? region, string? pais)
    {
        public string Alias { get; } = alias;
        public string IdentificacionTipoCodigo { get; } = identificacionTipoCodigo;
        public string IdentificacionValor { get; } = identificacionValor;
        public string? IdentificacionPais { get; } = identificacionPais;
        public string? RazonSocial { get; } = razonSocial;
        public string? Nombre { get; } = nombre;
        public string? PrimerApellido { get; } = primerApellido;
        public string? SegundoApellido { get; } = segundoApellido;
        public string Direccion { get; } = direccion;
        public string? CodigoPostal { get; } = codigoPostal;
        public string? Municipio { get; } = municipio;
        public string? Provincia { get; } = provincia;
        public string? Region { get; } = region;
        public string? Pais { get; } = pais;
    }
}
