using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class CrearClienteHandler(IClientesRepository repoCliente, IIdentificacionFiscalFactory identificacionFiscalFactory) 
    : ICommandHandler<CrearClienteCommand, Result<Guid>>
{
    private readonly IClientesRepository _repoCliente = repoCliente;
    private readonly IIdentificacionFiscalFactory _identificacionFiscalFactory = identificacionFiscalFactory;

    public async Task<Result<Guid>> HandleAsync(CrearClienteCommand command, CancellationToken ct)
    {
        var identificacionFiscalResult = _identificacionFiscalFactory.Crear(command.IdentificacionFiscalValor, command.IdentificacionFiscalTipo, command.IdentificacionFiscalCodigoPais);
        if (identificacionFiscalResult.IsFailure)
            return Result.Fail<Guid>(identificacionFiscalResult.Errors);

        var domicilioResult = Domicilio.Crear(command.Direccion, command.CodigoPostal, command.Municipio, 
                                                command.Provincia, command.Region, command.Pais);
        if (domicilioResult.IsFailure)
            return Result.Fail<Guid>(domicilioResult.Errors);

        // TODO: Pendiente de quitar de aquí e implementar tenantId desde claims con seguridad vía JWT access token, etc.
        var tenantIdResult = TenantId.Crear(3);

        var id = Guid.NewGuid();
        var clienteResult = Cliente.Crear(id, tenantIdResult.Value, command.Alias, identificacionFiscalResult.Value, command.RazonSocial,
                                            command.Nombre, command.PrimerApellido, command.SegundoApellido, domicilioResult.Value);
        if (clienteResult.IsFailure)
            return Result.Fail<Guid>(clienteResult.Errors);

        if (await _repoCliente.AliasEnUsoAsync(command.Alias, ct))
            return Result.Fail<Guid>(Error.Conflict("AliasExistente", "Ya existe un cliente con el mismo alias."));

        if (await _repoCliente.IdentificacionFiscalEnUsoAsync(command.IdentificacionFiscalValor, command.IdentificacionFiscalTipo, command.IdentificacionFiscalCodigoPais, ct))
            return Result.Fail<Guid>(Error.Conflict("IdentificacionFiscalExistente", "Ya existe un cliente con la misma identificación fiscal."));

        var cliente = clienteResult.Value;

        await _repoCliente.GuardarAsync(cliente, ct);

        return Result.Ok(cliente.Id);
    }
}
