using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public class IdentificacionFiscalFactory : IIdentificacionFiscalFactory
{
    public Result<IIdentificacionFiscal> Crear(
        string identificacionFiscalValor,
        TiposIdentificacionFiscal identificacionFiscalTipo,
        string? identificacionFiscalCodigoPais)
    {
        if (identificacionFiscalTipo == TiposIdentificacionFiscal.Nif)
        {
            var nifEspanol = NifEspanol.Crear(identificacionFiscalValor);
            return nifEspanol.IsSuccess
                ? Result.Ok<IIdentificacionFiscal>(nifEspanol.Value)
                : Result.Fail<IIdentificacionFiscal>(nifEspanol.Errors);
        }
        
        if (identificacionFiscalCodigoPais is not null)
        {
            var idOtroExtranjero = IdOtroExtranjero.Crear(identificacionFiscalValor, identificacionFiscalTipo, identificacionFiscalCodigoPais);
            return idOtroExtranjero.IsSuccess
                ? Result.Ok<IIdentificacionFiscal>(idOtroExtranjero.Value)
                : Result.Fail<IIdentificacionFiscal>(idOtroExtranjero.Errors);
        }

        return Result.Fail<IIdentificacionFiscal>(IdentificacionFiscalErrors.IdentificacionFiscalInvalida);
    }
}
