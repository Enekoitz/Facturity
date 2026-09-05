using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public sealed class ClienteRecord
{
    public Guid Id { get; init; }
    public string Alias { get; init; }
    public string IdentificacionFiscalValor { get; init; }
    public TiposIdentificacionFiscal IdentificacionFiscalTipo { get; init; }
    public string IdentificacionFiscalCodigoPais { get; init; }
    // si cambias RazonSocial o Nombre y apellidos, actualiza también la columna calculada DenominacionFiscalBusqueda en la configuración de EF Core
    public string? RazonSocial { get; init; }
    public string? Nombre { get; init; }
    public string? PrimerApellido { get; init; }
    public string? SegundoApellido { get; init; }
    public Domicilio? Domicilio { get; init; }
    public DateTimeOffset? ActivatedAtUtc { get; init; }
    public DateTimeOffset? DeactivatedAtUtc { get; init; }
    public DateTimeOffset? DeletedAtUtc { get; init; }
    public string DireccionBusqueda { get; init; }
    public string DenominacionFiscalBusqueda { get; init; }

    private ClienteRecord() {}

    public ClienteRecord(
        Guid id,
        string alias,
        string identificacionFiscalValor,
        TiposIdentificacionFiscal identificacionFiscalTipo,
        string identificacionFiscalCodigoPais,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Domicilio? domicilio,
        DateTimeOffset? activatedAtUtc,
        DateTimeOffset? deactivatedAtUtc,
        DateTimeOffset? deletedAtUtc)
    {
        Id = id;
        Alias = alias;
        IdentificacionFiscalValor = identificacionFiscalValor;
        IdentificacionFiscalTipo = identificacionFiscalTipo;
        IdentificacionFiscalCodigoPais =  identificacionFiscalCodigoPais;
        RazonSocial = razonSocial;
        Nombre = nombre;
        PrimerApellido = primerApellido;
        SegundoApellido = segundoApellido;
        Domicilio = domicilio;
        ActivatedAtUtc = activatedAtUtc;
        DeactivatedAtUtc = deactivatedAtUtc;
        DeletedAtUtc = deletedAtUtc;
    }
}