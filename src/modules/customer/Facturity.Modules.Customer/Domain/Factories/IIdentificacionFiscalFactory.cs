using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public interface IIdentificacionFiscalFactory
{
    Result<IIdentificacionFiscal> Crear(
        string? nif,
        string? idOtroExtranjeroValor,
        string? idOtroExtranjeroTipo,
        string? idOtroExtranjeroCodigoPais);
}