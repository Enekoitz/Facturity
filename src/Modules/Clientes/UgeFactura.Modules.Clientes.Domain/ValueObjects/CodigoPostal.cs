using UgeFactura.Common.Primitives;

namespace UgeFactura.Modules.Clientes.Domain
{
    public sealed record CodigoPostal
    {
        public string Valor { get; }

        private CodigoPostal() { }

        private CodigoPostal(string value)
        {
            Valor = value;
        }

        public static Result<CodigoPostal> Crear(string cp)
        {
            if (string.IsNullOrWhiteSpace(cp))
                return Result.Fail<CodigoPostal>(CodigoPostalCodigosError.CodigoPostal_Vacio.ToString());

            if (cp.Trim().Length != 5)
                return Result.Fail<CodigoPostal>(CodigoPostalCodigosError.CodigoPostal_LongitudInvalida.ToString());

            if (!cp.All(char.IsDigit))
                return Result.Fail<CodigoPostal>(CodigoPostalCodigosError.CodigoPostal_FormatoInvalido.ToString());

            return Result.Ok(new CodigoPostal(cp.Trim()));
        }

        public override string ToString() => Valor;
    }
}
