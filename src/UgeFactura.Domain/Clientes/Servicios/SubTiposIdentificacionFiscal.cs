using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Clientes
{
    public enum SubTipoIdentificacionFiscal
    {
        NifPersonaFisica,
        Nie,
        NifPersonaJuridica
    }

    public static class SubTiposIdentificacionFiscal
    {
        public static Result<SubTipoIdentificacionFiscal?> ObtenerSubTipo(string codigoIdentificacion, string valorIdentificacion)
        {
            if (string.IsNullOrWhiteSpace(codigoIdentificacion))
                return Result.Fail<SubTipoIdentificacionFiscal?>(IdentificacionFiscalCodigosError.TipoIdentificacionFiscal_Vacio.ToString());

            valorIdentificacion = valorIdentificacion.Trim().ToUpper();
            codigoIdentificacion = codigoIdentificacion.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(valorIdentificacion))
                return Result.Fail<SubTipoIdentificacionFiscal?>(IdentificacionFiscalCodigosError.ValorIdentificacionFiscal_Vacio.ToString());

            switch (codigoIdentificacion)
            {
                case "01":
                    if (ValidarIdentificacionFiscalUtils.EsNif(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacionFiscal?>(SubTipoIdentificacionFiscal.NifPersonaFisica);

                    if (ValidarIdentificacionFiscalUtils.EsNie(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacionFiscal?>(SubTipoIdentificacionFiscal.Nie);

                    if (ValidarIdentificacionFiscalUtils.EsCif(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacionFiscal?>(SubTipoIdentificacionFiscal.NifPersonaJuridica);

                    return Result.Fail<SubTipoIdentificacionFiscal?>(IdentificacionFiscalCodigosError.SubTipoIdentificacionFiscal_Invalido.ToString());

                default:
                    return Result.Ok<SubTipoIdentificacionFiscal?>(null);
            }
        }
    }
}
