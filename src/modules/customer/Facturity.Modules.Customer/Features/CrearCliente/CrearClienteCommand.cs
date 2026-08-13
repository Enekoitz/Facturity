using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal record CrearClienteCommand(
    string Alias,
    string IdentificacionValor,
    string IdentificacionTipo,
    string? IdentificacionCodigoPais,
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
