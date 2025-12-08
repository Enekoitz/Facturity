using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Cliente.DTOs;
using UgeFactura.Domain.Comun;
using UgeFactura.Domain.ValueObjects;

namespace UgeFactura.Domain.Cliente.Entidades
{
    internal class Cliente
    {
        private Guid Guid { get; set; }
        private string Alias { get; set; }
        private Identificacion Identificacion { get; set; }
        private string RazonSocial { get; set; }
        private string Nombre { get; set; }
        private string PrimerApellido { get; set; }
        private string SegundoApellido { get; set; }
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

        private bool ValidarNombreApellido(string nombre, string primerApellido, string segundoApellido, out List<string> errores)
        {
            if (errores == null)
            {
                errores
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                errores.Add("El nombre no puede estar vacío.");
                return false;
            }
            
            return true;
        }
    }
}
