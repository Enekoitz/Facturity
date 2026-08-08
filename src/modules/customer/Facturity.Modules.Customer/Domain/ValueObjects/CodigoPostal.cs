using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed class CodigoPostal : ValueObject
{
    public string Valor { get; }

    protected CodigoPostal() { }

    private CodigoPostal(string value)
    {
        Valor = value;
    }

    public static Result<CodigoPostal> Crear(string codigoPostal)
    {
        if (Validate.EsVacio(codigoPostal))
            return Result.Fail<CodigoPostal>(Error.Validation(CodigoPostalCodigosError.CodigoPostal_Vacio.ToString(), "El código postal no puede ser vacío."));

        if (Validate.NoEsLongitudExacta(codigoPostal, Parametros.CodigoPostalLongitudExacta))
            return Result.Fail<CodigoPostal>(Error.Validation(CodigoPostalCodigosError.CodigoPostal_LongitudInvalida.ToString(), "El código postal no puede ser vacío."));

        if (!codigoPostal.All(char.IsDigit))
            return Result.Fail<CodigoPostal>(Error.Validation(CodigoPostalCodigosError.CodigoPostal_FormatoInvalido.ToString(), "El código postal no puede ser vacío."));

        return Result<CodigoPostal>.Ok(new CodigoPostal(codigoPostal.Trim()));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Valor;
    }
}
