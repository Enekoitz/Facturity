using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public class ClienteRecord
{
    public Guid Id { get; set; }
    public string Alias { get; set; }
    public TiposIdentificacionFiscal TipoIdentificacionFiscal { get; set; }
    public string? Nif { get; set; }
    public string? IdOtroExtranjeroValor { get; set; }
    public TiposIdsOtrosExtranjeros? IdOtroExtranjeroTipo { get; set; }
    public string? IdOtroExtranjeroCodigoPais { get; set; }
    public string? RazonSocial { get; set; }
    public string? Nombre { get; set; }
    public string? PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }
    public Domicilio? Domicilio { get; set; }

    public ClienteRecord() { }

}