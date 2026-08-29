using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public static class ClienteMapper
{
    public static ClienteRecord ToRecord(this Cliente cliente)
    {

        (string valor, TiposIdentificacionFiscal tipo, string codigoPais) = cliente.IdentificacionFiscal.ObtenerValoresComunes();

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
            Domicilio = cliente.Domicilio,
            DeletedAtUtc = cliente.DeletedAtUtc,
        };
    }

    public static Cliente? FromRecord(this ClienteRecord clienteRecord, IIdentificacionFiscalFactory identificacionFiscalFactory)
    {

        var identificacionFiscalResult = identificacionFiscalFactory.Crear(clienteRecord.IdentificacionFiscalValor, 
            clienteRecord.IdentificacionFiscalTipo, clienteRecord.IdentificacionFiscalCodigoPais);
        
        if (identificacionFiscalResult.IsFailure)
            return null;

        var tenantIdResult = TenantId.Crear(3);

        var id = Guid.NewGuid();
        var clienteResult = Cliente.Reconstruir(id, tenantIdResult.Value, clienteRecord.Alias, identificacionFiscalResult.Value, 
            clienteRecord.RazonSocial,clienteRecord.Nombre, clienteRecord.PrimerApellido, clienteRecord.SegundoApellido, 
            clienteRecord.Domicilio, clienteRecord.DeletedAtUtc, TimeProvider.System);

        return clienteResult.Value;
    }

}
