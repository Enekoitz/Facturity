using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

internal static class ClienteErrors
{
    internal static readonly Error IdInvalido =
        Error.Validation("Cliente.IdInvalido", "El identificador del cliente no es válido.");

    internal static readonly Error FechaInvalida = Error.Unexpected("Cliente.FechaInvalida",
        "La fecha de eliminación no puede ser futura.");

    internal static readonly Error AliasInvalido =
        Error.Validation("Cliente.AliasInvalido", "El alias no puede ser vacío.");

    internal static readonly Error AliasDemasiadoLargo = Error.Validation(
        "Cliente.AliasDemasiadoLargo",
        $"El alias no puede superar los {ClienteParametros.AliasMaxLength} caracteres.");

    internal static readonly Error NombreVacio = Error.Validation(
        "Cliente.NombreVacio",
        "El nombre no puede ser vacío.");
    
    internal static readonly Error NombreDemasiadoLargo = Error.Validation(
        "Cliente.NombreDemasiadoLargo",
        $"El nombre no puede superar los {ClienteParametros.NombreMaxLength} caracteres.");

    internal static readonly Error PrimerApellidoVacio = Error.Validation(
        "Cliente.PrimerApellidoVacio", 
        "El primer apellido no puede ser vacío.");
    
    internal static readonly Error PrimerApellidoDemasiadoLargo = Error.Validation(
        "Cliente.PrimerApellidoDemasiadoLargo",
        $"El primer apellido no puede superar los {ClienteParametros.ApellidoMaxLength} caracteres.");
    
    internal static readonly Error SegundoApellidoDemasiadoLargo = Error.Validation(
        "Cliente.SegundoApellidoDemasiadoLargo",
        $"El segundo apellido no puede superar los {ClienteParametros.ApellidoMaxLength} caracteres.");

    internal static readonly Error RazonSocialVacio = Error.Validation(
        "Cliente.RazonSocialVacio",
        "La razon social no puede ser vacío.");

    internal static readonly Error RazonSocialDemasiadoLargo = Error.Validation(
        "Cliente.RazonSocialDemasiadoLargo",
        $"La razon social no puede superar los {ClienteParametros.RazonSocialMaxLength} caracteres.");
    
    internal static readonly Error NiRazonSocialNiNombreApellido = Error.Validation(
        "Cliente.NiRazonSocialNiNombreApellido",
        "No se ha informado la razón social ni el nombre y apellido.");
    
    internal static readonly Error NoEncontrado =
        Error.NotFound("Cliente.NoEncontrado", "El cliente especificado no existe en el sistema.");

    internal static readonly Error CorreoDuplicado =
        Error.Conflict("Cliente.CorreoDuplicado", "Ya existe un cliente registrado con este correo.");
    
    internal static readonly Error EliminadoPreviamente =
        Error.NotFound("Cliente.EliminadoPreviamente", "El cliente ha sido eliminado previamente.");
    
}