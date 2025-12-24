using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgeFactura.Domain.Cliente.ValueObjects
{
    public sealed record CodigoPostal
    {
        private string Valor { get; }

        private CodigoPostal(string value)
        {
            Valor = value;
        }

        public static Result<CodigoPostal> Crear(string cp)
        {
            if (string.IsNullOrWhiteSpace(cp))
                return Result.Fail<CodigoPostal>("El código postal no puede estar vacío.");

            if (cp.Trim().Length != 5)
                return Result.Fail<CodigoPostal>("El código postal debe tener una longitud de 5 dígitos.");

            if (!cp.All(char.IsDigit))
                return Result.Fail<CodigoPostal>("El código postal debe contener solo dígitos.");

            return Result.Ok(new CodigoPostal(cp.Trim()));
        }

        public override string ToString() => Valor;
    }

}
