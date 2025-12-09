using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Cliente.DTOs;
using UgeFactura.Domain.Cliente.ValueObjects;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Cliente.Entidades
{
    internal class Cliente
    {
        private Guid Guid { get; set; }
        private string Alias { get; set; }
        private Identificacion Identificacion { get; set; }
        private string? RazonSocial { get; set; }
        private string? Nombre { get; set; }
        private string? PrimerApellido { get; set; }
        private string? SegundoApellido { get; set; }
        private Domicilio Domicilio { get; set; }  

        private Cliente() {
            Guid = Guid.NewGuid();
        }

        public static Result<Cliente> Crear(ClienteParams p) {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(p.Alias))
                errores.Add("El alias no puede estar vacío.");

            var identificacionResult = Identificacion.Crear(p.Identificacion.CodigoTipo, p.Identificacion.Valor, p.Identificacion.PaisCodigo);

            if (identificacionResult.IsSuccess)
            {
                List<string> erroresNombreApellido;

                switch (identificacionResult.Value.SubTipo)
                {
                    case Servicios.SubTipoIdentificacion.NifPersonaFisica:
                        ValidarNombreApellido(p.Nombre, p.PrimerApellido, p.SegundoApellido, out erroresNombreApellido);
                        break;

                    case Servicios.SubTipoIdentificacion.Nie:
                        ValidarNombreApellido(p.Nombre, p.PrimerApellido, p.SegundoApellido, out erroresNombreApellido);
                        break;

                    case Servicios.SubTipoIdentificacion.NifPersonaJuridica:
                        ValidarRazonSocial(p.RazonSocial, out erroresNombreApellido);
                        break;

                    default:
                        if (!string.IsNullOrWhiteSpace(p.RazonSocial))
                        {
                            if (ValidarRazonSocial(p.RazonSocial, out erroresNombreApellido))
                                p.Nombre = p.PrimerApellido = p.SegundoApellido = null;
                        }
                        else 
                        {
                            if (ValidarNombreApellido(p.Nombre, p.PrimerApellido, p.SegundoApellido, out erroresNombreApellido))
                                p.RazonSocial = null;
                        }

                        erroresNombreApellido = new List<string>();
                        break;
                } 

                if (erroresNombreApellido.Count > 0)
                {
                    errores.AddRange(erroresNombreApellido);
                }

                if (errores.Count > 0)
                {
                    return Result.Fail(string.Join("; ", errores));
                }

                return Result.Ok(new Cliente
                {
                    Alias = p.Alias,
                    Identificacion = identificacionResult.Value,
                    RazonSocial = p.RazonSocial,
                    Nombre = p.Nombre,
                    PrimerApellido = p.PrimerApellido,
                    SegundoApellido = p.SegundoApellido,
                });
            }
            else
            {
                return Result.Fail(string.Join("; ", identificacionResult.Errors.Select(e => e.Message)));
            }
        }


        private static bool ValidarNombreApellido(
            string? nombre, 
            string? primerApellido, 
            string? segundoApellido, 
            out List<string> erroresNombreApellido)
        {
            erroresNombreApellido = new List<string>();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                erroresNombreApellido.Add("El nombre no puede estar vacío.");
            }
            else if(nombre.Length > 39)
            {
                erroresNombreApellido.Add("El nombre debe tener una longitud máxima de 20 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(primerApellido))
            {
                erroresNombreApellido.Add("El primer apellido no puede estar vacío.");
            }
            else if (primerApellido?.Length > 39)
            {
                erroresNombreApellido.Add("El primer apellido debe tener una longitud máxima de 20 caracteres.");
            }

            if (segundoApellido?.Length > 39)
            {
                erroresNombreApellido.Add("El segundo apellido debe tener una longitud máxima de 20 caracteres.");
            }

            if (erroresNombreApellido.Count > 0)
                return false;
            
            return true;
        }

        private static bool ValidarRazonSocial(
            string? razonSocial, 
            out List<string> erroresRazonSocial)
        {
            erroresRazonSocial = new List<string>();
            if (string.IsNullOrWhiteSpace(razonSocial))
            {
                erroresRazonSocial.Add("La razón social no puede estar vacía.");
            }
            else if (razonSocial.Length > 120)
            {
                erroresRazonSocial.Add("La razón social debe tener una longitud máxima de 100 caracteres.");
            }
            if (erroresRazonSocial.Count > 0)
                return false;
            return true;
        }
    }
}
