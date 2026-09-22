using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record CodigoPostal
{
    public string Valor { get; }

    private CodigoPostal() { }

    private CodigoPostal(string value)
    {
        Valor = value;
    }

    public static Result<CodigoPostal> Crear(string codigoPostal)
    {
        if (Validate.EsVacio(codigoPostal))
            return Result.Fail<CodigoPostal>(Error.Validation(nameof(CodigoPostalCodigosError.CodigoPostalVacio), "El código postal no puede ser vacío."));

        if (Validate.NoEsLongitudExacta(codigoPostal, DomicilioParametros.CodigoPostalLength))
            return Result.Fail<CodigoPostal>(Error.Validation(nameof(CodigoPostalCodigosError.CodigoPostalLongitudInvalida), "El código postal no puede ser vacío."));

        if (!codigoPostal.All(char.IsDigit))
            return Result.Fail<CodigoPostal>(Error.Validation(nameof(CodigoPostalCodigosError.CodigoPostalFormatoInvalido), "El código postal no puede ser vacío."));

        return Result<CodigoPostal>.Ok(new CodigoPostal(codigoPostal.Trim()));
    }
}
