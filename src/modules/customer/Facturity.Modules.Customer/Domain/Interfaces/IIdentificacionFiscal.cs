namespace Facturity.Modules.Customer.Domain;

public interface IIdentificacionFiscal
{
    TiposIdentificacionFiscal TipoIdentificacionFiscal { get; }
    bool? EsPersonaJuridica { get; }
}