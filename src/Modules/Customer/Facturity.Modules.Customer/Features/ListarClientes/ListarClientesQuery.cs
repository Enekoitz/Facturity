using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record ListarClientesQuery(
    string? Alias, 
    TiposIdentificacionFiscal? IdentificacionFiscalTipo, 
    string? IdentificacionFiscalValor, 
    string? IdentificacionFiscalCodigoPais,
    string? DenominacionFiscalBusqueda,
    string? DireccionBusqueda,
    bool? Activo,
    int Page,
    int Size) : IQuery<Result<IEnumerable<Cliente>>>;
