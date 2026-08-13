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
        var identificacionFiscalResult = _identificacionFiscalFactory.Crear(command.IdentificacionValor, command.IdentificacionTipo, command.IdentificacionCodigoPais);
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

        if (await _repoCliente.AliasEnUsoAsync(command.Alias, ct))
            return Result.Fail<Guid>(Error.Conflict("AliasExistente", "Ya existe un cliente con el mismo alias."));

        if (Enum.TryParse(command.IdentificacionTipo, out TiposIdentificacionFiscal tipoIdentificacionFiscal) 
            && await _repoCliente.IdentificacionFiscalEnUsoAsync(command.IdentificacionValor, tipoIdentificacionFiscal, command.IdentificacionCodigoPais, ct))
           return Result.Fail<Guid>(Error.Conflict("IdentificacionFiscalExistente", "Ya existe un cliente con la misma identificación fiscal."));

        Cliente cliente = clienteResult.Value;

        await _repoCliente.GuardarAsync(cliente);

        return Result.Ok(cliente.Id);
    }
}
