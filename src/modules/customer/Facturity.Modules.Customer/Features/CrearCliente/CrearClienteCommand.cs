using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

public record CrearClienteCommand(
    string Alias,
    string? Nif,
    string? IdOtroExtranjero,
    string? IdOtroExtranjeroTipo,
    string? IdOtroExtranjeroCodigoPais,
    string? RazonSocial,
    string? Nombre,
    string? PrimerApellido,
    string? SegundoApellido,
    string Direccion,
    string? CodigoPostal,
    string? Municipio,
    string? Provincia,
    string? Region,
    string? Pais
) : ICommand<Result<Guid>>;
