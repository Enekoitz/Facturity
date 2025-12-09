using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgeFactura.Domain.Cliente.ValueObjects
{
    public sealed record Domicilio
    {
        private string Direccion { get; }
        private CodigoPostal CP { get; }
        private string Municipio { get; }
        private string Provincia { get; }
        private string Region { get; }
        private string Pais { get; }

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

            var errores = new List<string>();

            if (string.IsNullOrEmpty(direccion))
                errores.Add("El domicilio no puede estar vacío.");

            if (string.IsNullOrEmpty(municipio))
                errores.Add("El municipio no puede estar vacío.");

            if (string.IsNullOrEmpty(provincia))
                errores.Add("La provincia no puede estar vacía.");

            if (string.IsNullOrEmpty(region))
                errores.Add("La región no puede estar vacía.");

            if (string.IsNullOrEmpty(pais))
                errores.Add("El país no puede estar vacío.");

            var cpResult = CodigoPostal.Crear(cp);
            if (cpResult.IsFailed)
                errores.AddRange(cpResult.Errors.Select(e => e.Message));

            if (errores.Count > 0)
                return Result.Fail<Domicilio>(string.Join("; ", errores));

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