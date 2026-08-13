using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public static class ClienteMapper
{
    public static ClienteRecord ToRecord(this Cliente cliente)
    {

        var (valor, tipo, codigoPais) = cliente.IdentificacionFiscal switch
        {
            NifEspanol identificacion => (identificacion.Valor, TiposIdentificacionFiscal.Nif, "ES"),
            IdOtroExtranjero identificacion => (identificacion.Valor, identificacion.Tipo, identificacion.CodigoPais),
            _ => throw new InvalidOperationException($"Tipo de identificación fiscal no soportado: {cliente.IdentificacionFiscal?.GetType().Name}")
        };

        return new ClienteRecord()
        {
            Id = cliente.Id,
            Alias = cliente.Alias,
            IdentificacionFiscalValor = valor,
            IdentificacionFiscalTipo = tipo,
            IdentificacionFiscalCodigoPais = codigoPais,
            RazonSocial = cliente.RazonSocial,
            Nombre = cliente.Nombre,
            PrimerApellido = cliente.PrimerApellido,
            SegundoApellido = cliente.SegundoApellido,
            Domicilio = cliente.Domicilio
        };
    }
    
}
