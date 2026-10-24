using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record PostalCode
{
    public string Valor { get; }

    private PostalCode() { }

    private PostalCode(string value)
    {
        Valor = value;
    }

    public static Result<PostalCode> Crear(string codigoPostal)
    {
        if (Validate.EsVacio(codigoPostal))
            return Result.Fail<PostalCode>(PostalCodeErrors.Empty);

        if (Validate.NoEsLongitudExacta(codigoPostal, Parameters.Address.PostalCodeExactLength))
            return Result.Fail<PostalCode>(PostalCodeErrors.TooLong);

        if (!codigoPostal.All(char.IsDigit))
            return Result.Fail<PostalCode>(PostalCodeErrors.NotNumber);

        return Result<PostalCode>.Ok(new PostalCode(codigoPostal.Trim()));
    }
}
