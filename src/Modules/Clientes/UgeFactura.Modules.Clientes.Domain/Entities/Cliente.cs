using UgeFactura.Common.Primitives;

namespace UgeFactura.Modules.Clientes.Domain
{
    public class Cliente
    {
        public Guid Id { get; init; }
        public string Alias { get; private set; }
        public IdentificacionFiscal Identificacion { get; private set; }
        public string? RazonSocial { get; private set; }
        public string? Nombre { get; private set; }
        public string? PrimerApellido { get; private set; }
        public string? SegundoApellido { get; private set; }
        public Domicilio Domicilio { get; private set; }

        protected Cliente() { }

        private Cliente(string alias,
            IdentificacionFiscal identificacionFiscal,
            string? razonSocial,
            string? nombre,
            string? primerApellido,
            string? segundoApellido,
            Domicilio domicilio) 
        {
            Id = Guid.NewGuid();
            Alias = alias;
            Identificacion = identificacionFiscal;
            RazonSocial = razonSocial;
            Nombre = nombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            Domicilio = domicilio;
        }

        public static Result<Cliente> Crear(
            string alias,
            IdentificacionFiscal identificacionFiscal,
            string? razonSocial,
            string? nombre,
            string? primerApellido,
            string? segundoApellido,
            Domicilio domicilio) 
        {
            var errores = new List<string>();

            ValidarAlias(alias, errores);
            SubtiposIdentificacionFiscalReglas? subtipoIdentificacionFiscal = SubtipoIdentificacionFiscalService.ObtenerSubtipo(identificacionFiscal.Tipo.Regla, identificacionFiscal.Valor);
            ValidarNombresApellidosRazonSocial(subtipoIdentificacionFiscal, razonSocial, nombre, primerApellido, segundoApellido, errores);

            if (errores.Count > 0)
                return Result.Fail<Cliente>(string.Join("; ", errores));

            return Result.Ok(new Cliente(
                                  alias,
                                  identificacionFiscal,
                                  razonSocial,
                                  nombre,
                                  primerApellido,
                                  segundoApellido,
                                  domicilio));
        }

        private static void ValidarAlias(
            string alias,
            List<string> errores)
        {
            TextoValidaciones.ValidarNoVacio(
                alias,
                ClienteCodigosError.Alias_Vacio,
                errores);

            TextoValidaciones.ValidarLongitud(
                alias,
                Parametros.AliasLongitudMaxima,
                ClienteCodigosError.Alias_DemasiadoLargo,
                errores);
        }

        private static void ValidarNombresApellidosRazonSocial(
            SubtiposIdentificacionFiscalReglas? subtipoIdentificacionFiscal,
            string? razonSocial,
            string? nombre,
            string? primerApellido,
            string? segundoApellido,
            List<string> errores)
        {
            switch (subtipoIdentificacionFiscal)
            {
                case SubtiposIdentificacionFiscalReglas.Nif:
                case SubtiposIdentificacionFiscalReglas.Nie:
                    ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                    break;

                case SubtiposIdentificacionFiscalReglas.Cif:
                    ValidarRazonSocial(razonSocial, errores);
                    break;

                default:
                    if (!string.IsNullOrWhiteSpace(razonSocial))
                        ValidarRazonSocial(razonSocial, errores);
                    else
                        ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                    break;
            }
        }

        private static void ValidarNombreApellido(
            string? nombre, 
            string? primerApellido, 
            string? segundoApellido, 
            List<string> errores)
        {
            TextoValidaciones.ValidarNoVacio(
                nombre,
                ClienteCodigosError.Nombre_Vacio,
                errores);

            TextoValidaciones.ValidarLongitud(
                nombre,
                Parametros.NombreLongitudMaxima,
                ClienteCodigosError.Nombre_DemasiadoLargo,
                errores);

            TextoValidaciones.ValidarNoVacio(
                primerApellido,
                ClienteCodigosError.PrimerApellido_Vacio,
                errores);

            TextoValidaciones.ValidarLongitud(
                primerApellido,
                Parametros.ApellidoLongitudMaxima,
                ClienteCodigosError.PrimerApellido_DemasiadoLargo,
                errores);

            if (!string.IsNullOrWhiteSpace(segundoApellido))
            {
                TextoValidaciones.ValidarLongitud(
                    segundoApellido,
                    Parametros.ApellidoLongitudMaxima,
                    ClienteCodigosError.SegundoApellido_DemasiadoLargo,
                    errores);
            }
        }

        private static void ValidarRazonSocial(
            string? razonSocial,
            List<string> errores)
        {
            TextoValidaciones.ValidarNoVacio(
                razonSocial,
                ClienteCodigosError.RazonSocial_Vacio,
                errores);

            TextoValidaciones.ValidarLongitud(
                razonSocial,
                Parametros.RazonSocialLongitudMaxima,
                ClienteCodigosError.RazonSocial_DemasiadoLargo,
                errores);
        }
    }
}