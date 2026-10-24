using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public static class ClienteMapper
{
    public static ClienteRecord ToRecord(this Domain.Customer customer)
    {

        (string valor, TaxIdType tipo, string codigoPais) = customer.TaxId.GetCommonValues();

        return new ClienteRecord(
            customer.Id,
            customer.Alias,
            valor,
            tipo,
            codigoPais,
            customer.RazonSocial,
            customer.Nombre,
            customer.PrimerApellido,
            customer.SegundoApellido,
            customer.Domicilio,
            customer.ActivatedAtUtc,
            customer.DeactivatedAtUtc,
            customer.DeletedAtUtc);
    }

    public static Domain.Customer? FromRecord(this ClienteRecord clienteRecord, ITaxIdFactory taxIdFactory)
    {

        var identificacionFiscalResult = taxIdFactory.Create(clienteRecord.IdentificacionFiscalValor, 
            clienteRecord.IdentificacionFiscalTipo, clienteRecord.IdentificacionFiscalCodigoPais);
        
        if (identificacionFiscalResult.IsFailure)
            return null;

        var tenantIdResult = TenantId.Crear(Guid.NewGuid());

        var clienteResult = Domain.Customer.Reconstruir(
            clienteRecord.Id, 
            tenantIdResult.Value, 
            clienteRecord.Alias, 
            identificacionFiscalResult.Value, 
            clienteRecord.RazonSocial,
            clienteRecord.Nombre, 
            clienteRecord.PrimerApellido, 
            clienteRecord.SegundoApellido,
            clienteRecord.Domicilio, 
            clienteRecord.ActivatedAtUtc,
            clienteRecord.DeactivatedAtUtc, 
            clienteRecord.DeletedAtUtc, 
            TimeProvider.System
        );

        return clienteResult.IsFailure 
            ? null 
            : clienteResult.Value;
    }
}
