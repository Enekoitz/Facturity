using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Clientes
{
    public sealed record IdentificacionFiscal
    {
        public TipoIdentificacionFiscal Tipo { get; private set; }
        public SubTipoIdentificacionFiscal? SubTipo { get; private set; }
        public string Valor { get; private set; }
        public string? PaisCodigo { get; private set; }

        private IdentificacionFiscal(TipoIdentificacionFiscal tipo, string valor, string? paisCodigo, SubTipoIdentificacionFiscal? subTipo)
        {
            Tipo = tipo;
            Valor = valor;
            PaisCodigo = paisCodigo;
            SubTipo = subTipo;
        }

        public static Result<IdentificacionFiscal> Crear(string tipoCodigo, string valor, string? paisCodigo = null)
        {
            tipoCodigo = tipoCodigo?.Trim().ToUpper() ?? string.Empty;
            valor = valor?.Trim().ToUpper() ?? string.Empty;
            paisCodigo = paisCodigo?.Trim().ToUpper();

            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(tipoCodigo))
                errores.Add("El código de tipo de identificación no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(valor))
                errores.Add("El valor de la identificación no puede estar vacío.");

            var tipoResult = TiposIdentificacionFiscal.Obtener(tipoCodigo);
            if (tipoResult.IsFailed)
                errores.AddRange(tipoResult.Errors.Select(e => e.Message));

            if (errores.Count > 0)
                return Result.Fail<IdentificacionFiscal>(string.Join("; ", errores));

            if (tipoResult.Value.RequierePais && !ValidarCodigos.EsCodigoPaisValido(paisCodigo, out string mensajeError))
                errores.Add(mensajeError);

            if (errores.Count > 0)
                return Result.Fail<IdentificacionFiscal>(string.Join("; ", errores));

            var subTipoResult = SubTiposIdentificacionFiscal.ObtenerSubTipo(tipoResult.Value.Codigo, valor);
            if (subTipoResult.IsFailed)
                errores.AddRange(subTipoResult.Errors.Select(e => e.Message));

            if (!tipoResult.Value.EsValido(valor, out mensajeError))
                return Result.Fail<IdentificacionFiscal>(mensajeError);

            return Result.Ok(new IdentificacionFiscal(tipoResult.Value, valor, paisCodigo, subTipoResult.Value));
        }

        public override string ToString() => Valor;
    }
}
