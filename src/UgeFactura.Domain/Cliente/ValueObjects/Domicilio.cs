using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun.Mensajes;

namespace UgeFactura.Domain.Cliente.ValueObjects
{
    public sealed record Domicilio
    {
        public string Direccion { get; private set; }
        public CodigoPostal CP { get; private set; }
        public string Municipio { get; private set; }
        public string Provincia { get; private set; }
        public string Region { get; private set; }
        public string Pais { get; private set; }

        private Domicilio(string domicilio, CodigoPostal cp, string municipio, string provincia, string region, string pais)
        {
            Direccion = domicilio; 
            CP = cp;
            Municipio = municipio;
            Provincia = provincia;
            Region = region;
            Pais = pais;
        }

        public static Result<Domicilio> Crear(string direccion, string cp, string municipio, string provincia, string region, string pais)
        {
            direccion = direccion.Trim();
            cp = cp.Trim();
            municipio = municipio.Trim();
            provincia = provincia.Trim();
            region = region.Trim(); 
            pais = pais.Trim();

            var erroresCodigos = new List<string>();

            if (string.IsNullOrWhiteSpace(direccion))
                erroresCodigos.Add(DomicilioCodigosError.Direccion_Vacio.ToString());

            if (string.IsNullOrWhiteSpace(municipio))
                erroresCodigos.Add(DomicilioCodigosError.Municipio_Vacio.ToString());

            if (string.IsNullOrWhiteSpace(provincia))
                erroresCodigos.Add(DomicilioCodigosError.Provincia_Vacio.ToString());

            if (string.IsNullOrWhiteSpace(region))
                erroresCodigos.Add(DomicilioCodigosError.Region_Vacio.ToString());

            if (string.IsNullOrWhiteSpace(pais))
                erroresCodigos.Add(DomicilioCodigosError.Pais_Vacio.ToString());

            var cpResult = CodigoPostal.Crear(cp);
            if (cpResult.IsFailed)
                erroresCodigos.AddRange(cpResult.Errors.Select(e => e.Message));

            if (erroresCodigos.Count > 0)
                return Result.Fail<Domicilio>(string.Join("; ", erroresCodigos));

            return Result.Ok(new Domicilio(
                direccion, 
                cpResult.Value, 
                municipio, 
                provincia, 
                region, 
                pais));
        }
    }
}