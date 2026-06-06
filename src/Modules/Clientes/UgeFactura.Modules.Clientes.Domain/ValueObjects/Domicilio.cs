using UgeFactura.Common.Primitives;

namespace UgeFactura.Modules.Clientes.Domain
{
    public sealed record Domicilio
    {
        public string Direccion { get; private set; }
        public CodigoPostal? CP { get; private set; }
        public string? Municipio { get; private set; }
        public string? Provincia { get; private set; }
        public string? Region { get; private set; }
        public string? Pais { get; private set; }

        //EF Core
        private Domicilio() { }

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

            municipio = string.IsNullOrWhiteSpace(municipio) ? null : municipio.Trim();
            provincia = string.IsNullOrWhiteSpace(provincia) ? null : provincia.Trim();
            region = string.IsNullOrWhiteSpace(region) ? null : region.Trim();
            pais = string.IsNullOrWhiteSpace(pais) ? null : pais.Trim();

            CodigoPostal? codigoPostalVO = null;
            if (!string.IsNullOrWhiteSpace(cp))
            {
                var cpResult = CodigoPostal.Crear(cp);
                if (cpResult.IsFailure)
                    erroresCodigos.Add(cpResult.Error);
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