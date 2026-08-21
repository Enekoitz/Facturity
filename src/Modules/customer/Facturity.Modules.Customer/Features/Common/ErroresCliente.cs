using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal static class ErroresCliente
{
    internal static readonly Error NoEncontrado =
        Error.NotFound("Cliente.NoEncontrado", "El cliente especificado no existe en el sistema.");

    internal static readonly Error CorreoDuplicado =
        Error.Conflict("Cliente.CorreoDuplicado", "Ya existe un cliente registrado con este correo.");
}