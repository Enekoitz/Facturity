using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Cliente.Servicios;
using UgeFactura.Domain.Comun;
using static UgeFactura.Domain.Comun.IdentificacionFiscalUtils;

namespace UgeFactura.Domain.ValueObjects
{
    public sealed record Identificacion
    {
        public TipoIdentificacion Tipo { get; private set; }
        public SubTipoIdentificacion? SubTipo { get; private set; }
        public string Valor { get; private set; }
        public string? PaisCodigo { get; private set; }

        private Identificacion(TipoIdentificacion tipo, string valor, string? paisCodigo)
        {
            Tipo = tipo;
            Valor = valor;
            PaisCodigo = paisCodigo;
        }

        public static Result<Identificacion> Crear(string tipoCodigo, string valor, string? paisCodigo = null)
        {
            tipoCodigo = tipoCodigo?.Trim().ToUpper() ?? string.Empty;
            valor = valor?.Trim().ToUpper() ?? string.Empty;
            paisCodigo = paisCodigo?.Trim().ToUpper();

            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(tipoCodigo))
                errores.Add("El código de tipo de identificación no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(valor))
                errores.Add("El valor de la identificación no puede estar vacío.");

            var tipoResult = TiposIdentificacion.Obtener(tipoCodigo);
            if (tipoResult.IsFailed)
                errores.AddRange(tipoResult.Errors.Select(e => e.Message));

            if (errores.Count > 0)
                return Result.Fail<Identificacion>(string.Join("; ", errores));

            if (identificacionResult.Value.Tipo.Codigo == "01")
            {
                var SubTipoIdentificacionFiscal = IdentificacionFiscalUtils.DetectarSubTipo(p.Identificacion.Valor);

                if (SubTipoIdentificacionFiscal == SubTipoIdentificacion.Nif)
                {

                }
                else if (SubTipoIdentificacionFiscal == SubTipoIdentificacion.Nie)
                {

                }
                else if (SubTipoIdentificacionFiscal == SubTipoIdentificacion.Cif)
                {

                }
                else
                {
                    errores.Add("La identificación fiscal no es un NIF, NIE o CIF válido.");
                }
            }

            if (errores.Count > 0)
                return Result.Fail<Identificacion>(string.Join("; ", errores));

            if (tipoResult.Value.RequierePais && !ValidadorCodigos.EsCodigoPaisValido(paisCodigo, out string mensajeError))
                errores.Add(mensajeError);

            if (errores.Count > 0)
                return Result.Fail<Identificacion>(string.Join("; ", errores));

            if (!tipoResult.Value.EsValido(valor, out mensajeError))
                return Result.Fail<Identificacion>(mensajeError);

            return Result.Ok(new Identificacion(tipoResult.Value, valor, paisCodigo));
        }

        public override string ToString() => Valor;
    }
}
