using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

internal static class DomicilioErrors
{
    internal static readonly Error DireccionVacio =
        Error.Validation("Cliente.Domicilio.DireccionVacio", 
            "La dirección no puede ser vacía.");
}