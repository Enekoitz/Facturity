using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public interface IIdentificacionFiscalFactory
{
    Result<IIdentificacionFiscal> Crear(
        string identificacionFiscalValor,
        string identificacionFiscalTipo,
        string? identificacionFiscalCodigoPais);
}