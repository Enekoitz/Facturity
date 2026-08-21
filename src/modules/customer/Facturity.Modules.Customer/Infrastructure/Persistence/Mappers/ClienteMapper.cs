using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

    public static Cliente? FromRecord(this ClienteRecord clienteRecord, IIdentificacionFiscalFactory identificacionFiscalFactory)
    {

        var identificacionFiscalResult = identificacionFiscalFactory.Crear(clienteRecord.IdentificacionFiscalValor, clienteRecord.IdentificacionFiscalTipo, clienteRecord.IdentificacionFiscalCodigoPais);
        if (identificacionFiscalResult.IsFailure)
            return null;

        var tenantIdResult = TenantId.Crear(3);

        var id = Guid.NewGuid();
        var clienteResult = Cliente.Crear(id, tenantIdResult.Value, clienteRecord.Alias, identificacionFiscalResult.Value, clienteRecord.RazonSocial,
                                            clienteRecord.Nombre, clienteRecord.PrimerApellido, clienteRecord.SegundoApellido, clienteRecord.Domicilio);

        return clien
    }

}
