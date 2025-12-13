using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Cliente.Servicios
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
            valorIdentificacion = valorIdentificacion.Trim().ToUpper();
            codigoIdentificacion = codigoIdentificacion.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(codigoIdentificacion))
                return Result.Fail<SubTipoIdentificacionFiscal?>("El código de tipo de identificación no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(valorIdentificacion))
                return Result.Fail<SubTipoIdentificacionFiscal?>("El valor de identificación no puede estar vacío.");

            switch (codigoIdentificacion)
            {
                case "01":
                    if (IdentificacionFiscalUtils.EsNif(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacionFiscal?>(SubTipoIdentificacionFiscal.NifPersonaFisica);

                    if (IdentificacionFiscalUtils.EsNie(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacionFiscal?>(SubTipoIdentificacionFiscal.Nie);

                    if (IdentificacionFiscalUtils.EsCif(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacionFiscal?>(SubTipoIdentificacionFiscal.NifPersonaJuridica);

                    return Result.Fail<SubTipoIdentificacionFiscal?>("No se ha podido determinar el subtipo de identificación.");

                default:
                    return Result.Ok<SubTipoIdentificacionFiscal?>(null);
            }
        }
    }
}
