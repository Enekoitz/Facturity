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

            var identificacionResult = Identificacion.Crear(p.Identificacion.CodigoTipo, p.Identificacion.Valor, p.Identificacion.PaisCodigo);

            if (identificacionResult.IsSuccess)
            {
                if(string.IsNullOrWhiteSpace(p.Alias))
                    errores.Add("El alias no puede estar vacío.");

                if (identificacionResult.Value.SubTipo.Value)

                ValidarNombreApellido(p.Nombre, p.PrimerApellido, p.SegundoApellido, out List<string> erroresNombreApellido);

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
                return Result.Fail(string.Join(", ", identificacionResult.Errors.Select(e => e.Message)));
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
            else if(nombre.Length > 20)
            {
                erroresNombreApellido.Add("El nombre debe tener una longitud máxima de 20 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(primerApellido))
            {
                erroresNombreApellido.Add("El primer apellido no puede estar vacío.");
            }
            else if (primerApellido?.Length > 20)
            {
                erroresNombreApellido.Add("El primer apellido debe tener una longitud máxima de 20 caracteres.");
            }

            if (segundoApellido?.Length > 20)
            {
                erroresNombreApellido.Add("El segundo apellido debe tener una longitud máxima de 20 caracteres.");
            }

            if (erroresNombreApellido.Count > 0)
                return false;
            
            return true;
        }
    }
}
