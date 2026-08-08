using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

public sealed class CrearClienteHandler : ICommandHandler<CrearClienteCommand, Result<Guid>>
{
    private readonly IClientesRepository _repoCliente;
    private readonly IIdentificacionFiscalFactory _identificacionFiscalFactory;

    public CrearClienteHandler(IClientesRepository repoCliente, IIdentificacionFiscalFactory identificacionFiscalFactory)
    {
        _repoCliente = repoCliente;
        _identificacionFiscalFactory = identificacionFiscalFactory;
    }

    public async Task<Result<Guid>> HandleAsync(CrearClienteCommand command, CancellationToken ct)
    {
        var identificacionFiscalResult = _identificacionFiscalFactory.Crear(command.Nif, command.IdOtroExtranjero, 
                                                                            command.IdOtroExtranjeroTipo, command.IdOtroExtranjeroCodigoPais);
        if (identificacionFiscalResult.IsFailure)
            return Result.Fail<Guid>(identificacionFiscalResult.Errors);

        var domicilioResult = Domicilio.Crear(command.Direccion, command.CodigoPostal, command.Municipio, 
                                                command.Provincia, command.Region, command.Pais);
        if (domicilioResult.IsFailure)
            return Result.Fail<Guid>(domicilioResult.Errors);

        // TODO: Pendiente de quitar de aquí e implementar tenantId desde claims con seguridad vía JWT access token, etc.
        var tenantIdResult = TenantId.Crear(3);

        Guid id = Guid.NewGuid();
        var clienteResult = Cliente.Crear(id, tenantIdResult.Value, command.Alias, identificacionFiscalResult.Value, command.RazonSocial,
                                            command.Nombre, command.PrimerApellido, command.SegundoApellido, domicilioResult.Value);
        if (clienteResult.IsFailure)
            return Result.Fail<Guid>(clienteResult.Errors);

        if (await _repoCliente.ExisteAliasAsync(command.Alias, ct))
            return Result.Fail<Guid>(Error.Conflict("AliasExistente", "Ya existe un cliente con el mismo alias."));

        //if (await _repoCliente.ExisteIdentificacionFiscalAsync(command.IdentificacionTipoCodigo, command.IdentificacionValor, ct))
        //    return Result.Fail<Guid>(Error.Conflict("IdentificacionFiscalExistente", "Ya existe un cliente con la misma identificación fiscal."));

        Cliente cliente = clienteResult.Value;

        await _repoCliente.GuardarAsync(cliente);

        return Result.Ok(cliente.Id);
    }
}
