using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class ActualizarClienteHandler(IClientesRepository repo, IIdentificacionFiscalFactory identificacionFiscalFactory) 
    : ICommandHandler<ActualizarClienteCommand, Result>
{
    public async Task<Result> HandleAsync(ActualizarClienteCommand command, CancellationToken ct)
    {
        var cliente = await repo.ObtenerClientePorIdAsync(command.Id, ct);
        if (cliente == null)
            return Result.Fail(ClienteErrors.NoEncontrado);

        Result<IIdentificacionFiscal>? identificacionFiscalResult = null;
        if (command.IdentificacionFiscalValor is not null && command.IdentificacionFiscalTipo is not null)
        {
            identificacionFiscalResult = identificacionFiscalFactory.Crear(
                command.IdentificacionFiscalValor, 
                command.IdentificacionFiscalTipo.Value, 
                command.IdentificacionFiscalCodigoPais)!;
            
            if (identificacionFiscalResult.IsFailure)
                return Result.Fail(identificacionFiscalResult.Errors);
            
            var identificacionFiscalEnUso = await repo.IdentificacionFiscalEnUsoAsync(
                command.IdentificacionFiscalValor,
                command.IdentificacionFiscalTipo.Value, 
                command.IdentificacionFiscalCodigoPais, 
                ct)!;
            
            if (identificacionFiscalEnUso)
                return Result.Fail(ClienteErrors.IdentificacionFiscalEnUso);
        }

        Result<Domicilio>? domicilioResult = null;
        if (command.IdentificacionFiscalValor is not null && command.IdentificacionFiscalTipo is not null)
        {
            domicilioResult = Domicilio.Crear(command.Direccion!, command.CodigoPostal, command.Municipio, 
                command.Provincia, command.Region, command.Pais);
        
            if (domicilioResult.IsFailure)
                return Result.Fail(domicilioResult.Errors);
        }

        var identificacionFiscal = identificacionFiscalResult?.Value;
        var domicilio = domicilioResult?.Value;
        
        var result = cliente.Actualizar(identificacionFiscal, command.RazonSocial,
                                        command.Nombre, command.PrimerApellido, command.SegundoApellido, 
                                        domicilio);
        
        if (result.IsFailure)
            return result;

        await repo.ActualizarAsync(cliente, ct);

        return Result.Ok();
    }
}
