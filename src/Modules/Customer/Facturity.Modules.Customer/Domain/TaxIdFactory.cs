using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public class TaxIdFactory : ITaxIdFactory
{
    public Result<ITaxId> Create(
        string identificacionFiscalValor,
        TaxIdType identificacionFiscalTipo,
        string? identificacionFiscalCodigoPais)
    {
        if (identificacionFiscalTipo == TaxIdType.TaxId)
        {
            var nifEspanol = SpanishTaxId.Crear(identificacionFiscalValor);
            return nifEspanol.IsSuccess
                ? Result.Ok<ITaxId>(nifEspanol.Value)
                : Result.Fail<ITaxId>(nifEspanol.Errors);
        }
        
        if (identificacionFiscalCodigoPais is not null)
        {
            var idOtroExtranjero = OtherTaxId.Create(identificacionFiscalValor, identificacionFiscalTipo, identificacionFiscalCodigoPais);
            return idOtroExtranjero.IsSuccess
                ? Result.Ok<ITaxId>(idOtroExtranjero.Value)
                : Result.Fail<ITaxId>(idOtroExtranjero.Errors);
        }

        return Result.Fail<ITaxId>(TaxIdErrors.Invalid);
    }
}
