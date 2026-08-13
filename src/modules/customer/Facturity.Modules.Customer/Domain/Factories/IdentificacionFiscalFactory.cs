using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public class IdentificacionFiscalFactory : IIdentificacionFiscalFactory
{
    public Result<IIdentificacionFiscal> Crear(
        string identificacionFiscalValor,
        string identificacionFiscalTipo,
        string? identificacionFiscalCodigoPais)
    {
        if (!Enum.TryParse(identificacionFiscalTipo, out TiposIdentificacionFiscal tipoIdentificacionFiscal))
            Result.Fail<IIdentificacionFiscal>(Error.Validation("TipoIdentificacionFiscalInvalido", "El tipo de identificación fiscal proporcionado es inválido."));

        if (tipoIdentificacionFiscal == TiposIdentificacionFiscal.Nif)
        {
            var nifEspanol = NifEspanol.Crear(identificacionFiscalValor);
            return nifEspanol.IsSuccess
                ? Result.Ok<IIdentificacionFiscal>(nifEspanol.Value)
                : Result.Fail<IIdentificacionFiscal>(nifEspanol.Errors);
        }
        else if (identificacionFiscalCodigoPais is not null)
        {
            var idOtroExtranjero = IdOtroExtranjero.Crear(identificacionFiscalValor, tipoIdentificacionFiscal, identificacionFiscalCodigoPais);
            return idOtroExtranjero.IsSuccess
                ? Result.Ok<IIdentificacionFiscal>(idOtroExtranjero.Value)
                : Result.Fail<IIdentificacionFiscal>(idOtroExtranjero.Errors);
        }

        return Result.Fail<IIdentificacionFiscal>(Error.NotFound("IdentificacionFiscalInvalida", "No se ha detectado un tipo de identificación fiscal para los parámetros proporcionados."));
    }
}
