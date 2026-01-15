using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Clientes;

namespace UgeFactura.Application.Clientes.Crear
{
    public sealed class CrearClienteHandler
    {
        private readonly IClienteRepository _repoCliente;

        public CrearClienteHandler(IClienteRepository repoCliente)
        {
            _repoCliente = repoCliente;
        }

        public async Task<Result> Handle(CrearClienteCommand command)
        {
            if (await _repoCliente.ExisteAliasAsync(command.Alias))
                return Result.Fail("Ya existe un cliente con el mismo alias.");

            if (await _repoCliente.ExisteIdentificacionFiscalAsync(command.IdentificacionTipoCodigo
                                                                    , command.IdentificacionValor))
                return Result.Fail("Ya existe un cliente con la misma identificación fiscal.");

            Result<IdentificacionFiscal> identificacionFiscalResult = IdentificacionFiscal.Crear(command.IdentificacionTipoCodigo
                                                                                                , command.IdentificacionValor
                                                                                                , command.IdentificacionPais);
            if (identificacionFiscalResult.IsFailed)
                return Result.Fail(string.Join("; ", identificacionFiscalResult.Errors));

            IdentificacionFiscal identificacionFiscal = identificacionFiscalResult.Value;

            Result<Domicilio> domicilioResult = Domicilio.Crear(command.Direccion
                                                                , command.CodigoPostal
                                                                , command.Municipio
                                                                , command.Provincia
                                                                , command.Region
                                                                , command.Pais);
            if (domicilioResult.IsFailed)
                return Result.Fail(string.Join("; ", domicilioResult.Errors));

            Domicilio domicilio = domicilioResult.Value;

            Result<Cliente> clienteResult = Cliente.Crear(command.Alias
                                                , identificacionFiscal
                                                , command.RazonSocial
                                                , command.Nombre
                                                , command.PrimerApellido
                                                , command.SegundoApellido
                                                , domicilio);
            if (clienteResult.IsFailed)
                return Result.Fail(string.Join("; ", clienteResult.Errors));

            Cliente cliente = clienteResult.Value;

            await _repoCliente.AddAsync(cliente);

            return Result.Ok();
        }
    }
}
