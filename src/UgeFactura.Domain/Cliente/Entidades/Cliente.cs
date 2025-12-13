using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Cliente.Servicios;
using UgeFactura.Domain.Cliente.ValueObjects;
using UgeFactura.Domain.Comun;
using UgeFactura.Domain.Comun.Limites;
using UgeFactura.Domain.Comun.Mensajes;
using UgeFactura.Domain.Comun.Validaciones;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UgeFactura.Domain.Cliente.Entidades
{
    public class Cliente
    {
        public Guid Id { get; init; }
        public string Alias { get; private set; }
        public IdentificacionFiscal Identificacion { get; private set; }
        public string? RazonSocial { get; private set; }
        public string? Nombre { get; private set; }
        public string? PrimerApellido { get; private set; }
        public string? SegundoApellido { get; private set; }
        public Domicilio Domicilio { get; private set; }  

        private Cliente(string alias,
            IdentificacionFiscal identificacionFiscal,
            string? razonSocial,
            string? nombre,
            string? primerApellido,
            string? segundoApellido,
            Domicilio domicilio) 
        {
            Id = Guid.NewGuid();
            Alias = alias;
            Identificacion = identificacionFiscal;
            RazonSocial = razonSocial;
            Nombre = nombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            Domicilio = domicilio;
        }

        public static Result<Cliente> Crear(
            string alias,
            IdentificacionFiscal identificacionFiscal,
            string? razonSocial,
            string? nombre,
            string? primerApellido,
            string? segundoApellido,
            Domicilio domicilio) 
        {
            var errores = new List<string>();
            
            ValidarDatos(alias, identificacionFiscal.SubTipo, razonSocial, nombre, primerApellido, segundoApellido, errores);

            if (errores.Count > 0)
                return Result.Fail(string.Join("; ", errores));

            return Result.Ok(new Cliente(
                                  alias,
                                  identificacionFiscal,
                                  razonSocial,
                                  nombre,
                                  primerApellido,
                                  segundoApellido,
                                  domicilio));
        }

        internal static void ValidarDatos(
            string alias,
            SubTipoIdentificacionFiscal? subIdentificacionFiscal,
            string? razonSocial,
            string? nombre,
            string? primerApellido,
            string? segundoApellido,
            List<string> errores)
        {
            ValidarAlias(alias, errores);

            switch (subIdentificacionFiscal)
            {
                case SubTipoIdentificacionFiscal.NifPersonaFisica:
                case SubTipoIdentificacionFiscal.Nie:
                    ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                    break;

                case SubTipoIdentificacionFiscal.NifPersonaJuridica:
                    ValidarRazonSocial(razonSocial, errores);
                    break;

                default:
                    if (!string.IsNullOrWhiteSpace(razonSocial))
                        ValidarRazonSocial(razonSocial, errores);
                    else
                        ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                    break;
            }
        }

        internal static void ValidarAlias(
            string alias, 
            List<string> errores)
        {
            ValidarTexto.ValidarNoVacio(
                alias,
                CodigoError.AliasVacio,
                errores);

            ValidarTexto.ValidarLongitud(
                alias,
                LimiteCliente.AliasLongitudMaxima,
                CodigoError.AliasDemasiadoLargo,
                errores);
        }

        private static void ValidarNombreApellido(
            string? nombre, 
            string? primerApellido, 
            string? segundoApellido, 
            List<string> errores)
        {
            ValidarTexto.ValidarNoVacio(
                nombre,
                CodigoError.NombreVacio,
                errores);

            ValidarTexto.ValidarLongitud(
                nombre,
                LimiteCliente.NombreLongitudMaxima,
                CodigoError.NombreDemasiadoLargo,
                errores);

            ValidarTexto.ValidarNoVacio(
                primerApellido,
                CodigoError.PrimerApellidoVacio,
                errores);

            ValidarTexto.ValidarLongitud(
                primerApellido,
                LimiteCliente.ApellidoLongitudMaxima,
                CodigoError.PrimerApellidoDemasiadoLargo,
                errores);

            if (!string.IsNullOrWhiteSpace(segundoApellido))
            {
                ValidarTexto.ValidarLongitud(
                    segundoApellido,
                    LimiteCliente.ApellidoLongitudMaxima,
                    CodigoError.SegundoApellidoDemasiadoLargo,
                    errores);
            }
        }

        private static void ValidarRazonSocial(
            string? razonSocial,
            List<string> errores)
        {
            ValidarTexto.ValidarNoVacio(
                razonSocial,
                CodigoError.RazonSocialVacio,
                errores);

            ValidarTexto.ValidarLongitud(
                razonSocial,
                LimiteCliente.RazonSocialLongitudMaxima,
                CodigoError.RazonSocialDemasiadoLargo,
                errores);
        }
    }
}