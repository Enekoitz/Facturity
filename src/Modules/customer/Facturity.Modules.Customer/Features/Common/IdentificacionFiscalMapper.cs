using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Features;

internal static class IdentificacionFiscalMapper
{
    internal static (string Valor, TiposIdentificacionFiscal Tipo, string CodigoPais) ObtenerValoresComunes(this IIdentificacionFiscal identificacionFiscal)
    {
        (string valor, TiposIdentificacionFiscal tipo, string codigoPais) = identificacionFiscal switch
        {
            NifEspanol identificacion => (identificacion.Valor, TiposIdentificacionFiscal.Nif, "ES"),
            IdOtroExtranjero identificacion => (identificacion.Valor, identificacion.Tipo, identificacion.CodigoPais),
            _ => throw new InvalidOperationException($"Tipo de identificación fiscal no soportado: {identificacionFiscal?.GetType().Name}")
        };
        
        return  (valor, tipo, codigoPais);
    }
}