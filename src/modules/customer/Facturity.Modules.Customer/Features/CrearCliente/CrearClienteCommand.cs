using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal record CrearClienteCommand(
    string Alias,
    string IdentificacionFiscalValor,
    TiposIdentificacionFiscal IdentificacionFiscalTipo,
    string? IdentificacionFiscalCodigoPais,
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
