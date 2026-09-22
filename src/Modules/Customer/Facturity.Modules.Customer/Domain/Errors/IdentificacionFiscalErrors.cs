using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

internal static class IdentificacionFiscalErrors
{
    internal static readonly Error IdentificacionFiscalInvalida = Error.Validation(
        "Cliente.IdentificacionFiscalInvalida",
        "Los datos de identificación fiscal son inválidos.");
}