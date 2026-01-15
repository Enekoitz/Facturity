using FluentResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Clientes
{
    public sealed record Domicilio
    {
        public string Direccion { get; private set; }
        public CodigoPostal? CP { get; private set; }
        public string? Municipio { get; private set; }
        public string? Provincia { get; private set; }
        public string? Region { get; private set; }
        public string? Pais { get; private set; }

        private Domicilio(
            string direccion
            , CodigoPostal? cp
            , string? municipio
            , string? provincia
            , string? region
            , string? pais)
        {
            Direccion = direccion; 
            CP = cp;
            Municipio = municipio;
            Provincia = provincia;
            Region = region;
            Pais = pais;
        }

        // ⚠️ IMPORTANTE:
        // No introducir Value Objects aquí sin un cambio explícito
        // en los requisitos del dominio.
        public static Result<Domicilio> Crear(
            string direccion
            , string? cp
            , string? municipio                       
            , string? provincia
            , string? region
            , string? pais)
        {
            var erroresCodigos = new List<string>();

            if (string.IsNullOrWhiteSpace(direccion))
                erroresCodigos.Add(DomicilioCodigosError.Direccion_Vacio.ToString());

            // Normalizamos opcionales: null si vacíos
            municipio = string.IsNullOrWhiteSpace(municipio) ? null : municipio.Trim();
            provincia = string.IsNullOrWhiteSpace(provincia) ? null : provincia.Trim();
            region = string.IsNullOrWhiteSpace(region) ? null : region.Trim();
            pais = string.IsNullOrWhiteSpace(pais) ? null : pais.Trim();

            // Gestionamos el CP como VO opcional
            CodigoPostal? codigoPostalVO = null;
            if (!string.IsNullOrWhiteSpace(cp))
            {
                var cpResult = CodigoPostal.Crear(cp);
                if (cpResult.IsFailed)
                    erroresCodigos.AddRange(cpResult.Errors.Select(e => e.Message));
                else
                    codigoPostalVO = cpResult.Value;
            }

            if (erroresCodigos.Count > 0)
                return Result.Fail<Domicilio>(string.Join("; ", erroresCodigos));

            direccion = direccion.Trim();

            return Result.Ok(new Domicilio(
                direccion,
                codigoPostalVO, 
                municipio, 
                provincia, 
                region, 
                pais));
        }
    }
}