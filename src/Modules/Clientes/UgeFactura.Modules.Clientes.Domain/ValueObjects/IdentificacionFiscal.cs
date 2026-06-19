using UgeFactura.Common.Primitives;
namespace UgeFactura.Modules.Clientes.Domain
{
    public sealed class IdentificacionFiscal
    {
        public TipoIdentificacionFiscal Tipo { get; private set; }
        public string Valor { get; private set; }
        public string? PaisCodigo { get; private set; }

        private IdentificacionFiscal() { }

        private IdentificacionFiscal(TipoIdentificacionFiscal tipo, string valor, string? paisCodigo)
        {
            Tipo = tipo;
            Valor = valor;
            PaisCodigo = paisCodigo;
        }

        public static Result<IdentificacionFiscal> Crear(string tipoCodigo, IdentificacionFiscalReglasEnum tipoRegla, string valor, string? paisCodigo = null)
        {
            tipoCodigo = tipoCodigo?.Trim().ToUpper() ?? string.Empty;
            valor = valor?.Trim().ToUpper() ?? string.Empty;
            paisCodigo = paisCodigo?.Trim().ToUpper();

            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(tipoCodigo))
                errores.Add("El código de tipo de identificación no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(valor))
                errores.Add("El valor de la identificación no puede estar vacío.");

            if (errores.Count > 0)
                return Result.Fail<IdentificacionFiscal>(string.Join("; ", errores));

            bool requierePais = paisCodigo is not null;

            var tipoIdentificacionFiscal = new TipoIdentificacionFiscal(tipoCodigo, tipoRegla, requierePais);
            if (tipoIdentificacionFiscal.RequierePais && !CodesValidations.EsCodigoPaisValido(paisCodigo, out string mensajeError))
                errores.Add(mensajeError);

            if (!tipoIdentificacionFiscal.EsValido(valor, out mensajeError))
                return Result.Fail<IdentificacionFiscal>(mensajeError);

            return Result.Ok(new IdentificacionFiscal(tipoIdentificacionFiscal, valor, paisCodigo));
        }

        public override string ToString() => Valor;
    }
}
