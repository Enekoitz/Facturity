using UgeFactura.Modules.Clientes.Domain;
using UgeFactura.Modules.DomainSharedKernel;

namespace UgeFactura.Modules.Clientes.Features
{
    internal sealed class CrearClienteHandler
    {
        private readonly IClientesRepository _repoCliente;

        public CrearClienteHandler(IClientesRepository repoCliente)
        {
            _repoCliente = repoCliente;
        }

        public async Task<Result<Guid>> HandleAsync(CrearClienteCommand command)
        {
            if (await _repoCliente.ExisteAliasAsync(command.Alias))
                return Result.Fail<Guid>(Error.Conflict("01", "Ya existe un cliente con el mismo alias."));

            if (await _repoCliente.ExisteIdentificacionFiscalAsync(command.IdentificacionTipoCodigo
                                                                    , command.IdentificacionValor))
                return Result.Fail<Guid>(Error.Conflict("01", "Ya existe un cliente con la misma identificación fiscal."));

            TipoIdentificacionFiscal? tipoIdentificacion = await _repoCliente.ObtenerTipoIdentificacionAsync(command.IdentificacionTipoCodigo);

            if (tipoIdentificacion is null)
                return Result.Fail<Guid>(Error.Validation("02", "El código de tipo de identificación no existe o es incorrecto."));

            Result<IdentificacionFiscal> identificacionFiscalResult = IdentificacionFiscal.Crear(command.IdentificacionTipoCodigo
                                                                                                , tipoIdentificacion.Regla
                                                                                                , command.IdentificacionValor
                                                                                                , command.IdentificacionPais);
            if (identificacionFiscalResult.IsFailure)
                return Result.Fail<Guid>(identificacionFiscalResult.Errors);

            IdentificacionFiscal identificacionFiscal = identificacionFiscalResult.Value;


            Result<Domicilio>? domicilioResult = null;
            Domicilio? domicilio = null;
            if (command.Direccion is not null)
            {
                domicilioResult = Domicilio.Crear(command.Direccion
                                                , command.CodigoPostal
                                                , command.Municipio
                                                , command.Provincia
                                                , command.Region
                                                , command.Pais);

                if (domicilioResult.IsFailure)
                    return Result.Fail<Guid>(domicilioResult.Errors);

                domicilio = domicilioResult.Value;
            }

            Result<Cliente> clienteResult = Cliente.Crear(Guid.NewGuid()
                                                , command.Alias
                                                , identificacionFiscal
                                                , command.RazonSocial
                                                , command.Nombre
                                                , command.PrimerApellido
                                                , command.SegundoApellido
                                                , domicilio);
            if (clienteResult.IsFailure)
                return Result.Fail<Guid>(clienteResult.Errors);

            Cliente cliente = clienteResult.Value;

            await _repoCliente.GuardarAsync(cliente);

            return Result.Ok(cliente.Id);
        }
    }
}
