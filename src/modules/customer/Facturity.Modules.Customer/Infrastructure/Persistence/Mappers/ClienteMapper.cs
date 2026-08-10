using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public static class ClienteMapper
{
    public static ClienteRecord ToRecord(this Cliente cliente)
    {
        TiposIdentificacionFiscal tipoIdentificacionFiscal;
        string? nif = null;
        string? idOtroExtranjeroValor = null;
        TiposIdsOtrosExtranjeros? idOtroExtranjeroTipo = null;
        string? idOtroExtranjeroCodigoPais = null;

        if (cliente.IdentificacionFiscal is NifEspañol nifEspañol)
        {
            tipoIdentificacionFiscal = TiposIdentificacionFiscal.NifEspañol;
            nif = nifEspañol.Valor;
        } 
        else if (cliente.IdentificacionFiscal is IdOtroExtranjero idOtroExtranjero)
        {
            tipoIdentificacionFiscal = TiposIdentificacionFiscal.IdOtroExtranjero;
            idOtroExtranjeroValor = idOtroExtranjero.Valor;
            idOtroExtranjeroTipo = idOtroExtranjero.Tipo;
            idOtroExtranjeroCodigoPais = idOtroExtranjero.CodigoPais;
        }
        else
        {
            throw new Exception("No ha sido posible determinar el tipo de identificación fiscal del cliente.");
        }

        return new ClienteRecord()
        {
            Id = cliente.Id,
            Alias = cliente.Alias,
            TipoIdentificacionFiscal = tipoIdentificacionFiscal,
            Nif = nif,
            IdOtroExtranjeroValor = idOtroExtranjeroValor,
            IdOtroExtranjeroTipo = idOtroExtranjeroTipo,
            IdOtroExtranjeroCodigoPais = idOtroExtranjeroCodigoPais,
            RazonSocial = cliente.RazonSocial,
            Nombre = cliente.Nombre,
            PrimerApellido = cliente.PrimerApellido,
            SegundoApellido = cliente.SegundoApellido,
            Domicilio = cliente.Domicilio
        };
    }
}
