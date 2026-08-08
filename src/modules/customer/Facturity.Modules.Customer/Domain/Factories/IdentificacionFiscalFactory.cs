using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public class IdentificacionFiscalFactory : IIdentificacionFiscalFactory
{
    public Result<IIdentificacionFiscal> Crear(
        string? nif,
        string? idOtroExtranjeroValor,
        string? idOtroExtranjeroTipo,
        string? idOtroExtranjeroCodigoPais)
    {
        if (!string.IsNullOrWhiteSpace(nif) 
            && string.IsNullOrWhiteSpace(idOtroExtranjeroValor) 
            && string.IsNullOrWhiteSpace(idOtroExtranjeroTipo) 
            && string.IsNullOrWhiteSpace(idOtroExtranjeroCodigoPais))
        {
            var nifEspañol = NifEspañol.Crear(nif);
            return nifEspañol.IsSuccess
                ? Result.Ok<IIdentificacionFiscal>(nifEspañol.Value)
                : Result.Fail<IIdentificacionFiscal>(nifEspañol.Errors);
        }

        else if (!string.IsNullOrWhiteSpace(nif) 
            && !string.IsNullOrWhiteSpace(idOtroExtranjeroValor) 
            && !string.IsNullOrWhiteSpace(idOtroExtranjeroTipo) 
            && !string.IsNullOrWhiteSpace(idOtroExtranjeroCodigoPais))
        {
            TiposIdsOtrosExtranjeros? tipoIdentificacionFiscal = idOtroExtranjeroTipo.ParseToNullableEnum<TiposIdsOtrosExtranjeros>();
            if (tipoIdentificacionFiscal is not null)
            {
                var idOtroExtranjero = IdOtroExtranjero.Crear(idOtroExtranjeroValor, (TiposIdsOtrosExtranjeros)tipoIdentificacionFiscal, idOtroExtranjeroCodigoPais);
                return idOtroExtranjero.IsSuccess
                    ? Result.Ok<IIdentificacionFiscal>(idOtroExtranjero.Value)
                    : Result.Fail<IIdentificacionFiscal>(idOtroExtranjero.Errors);
            }
        }

        return Result.Fail<IIdentificacionFiscal>(Error.NotFound("IdentificacionFiscalInvalida", "No se ha detectado un tipo de identificación fiscal para los parámetros proporcionados."));
    }
}
