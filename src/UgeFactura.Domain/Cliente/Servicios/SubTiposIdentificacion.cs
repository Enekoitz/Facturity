using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Cliente.Servicios
{
    public enum SubTipoIdentificacion
    {
        NifPersonaFisica,
        Nie,
        NifPersonaJuridica
    }

    public static class SubTiposIdentificacion
    {
        public static Result<SubTipoIdentificacion?> ObtenerSubTipo(string codigoIdentificacion, string valorIdentificacion)
        {
            valorIdentificacion = valorIdentificacion.Trim().ToUpper();
            codigoIdentificacion = codigoIdentificacion.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(codigoIdentificacion))
                return Result.Fail<SubTipoIdentificacion?>("El código de tipo de identificación no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(valorIdentificacion))
                return Result.Fail<SubTipoIdentificacion?>("El valor de identificación no puede estar vacío.");

            switch (codigoIdentificacion)
            {
                case "01":
                    if (IdentificacionFiscalUtils.EsNif(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacion?>(SubTipoIdentificacion.NifPersonaFisica);

                    if (IdentificacionFiscalUtils.EsNie(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacion?>(SubTipoIdentificacion.Nie);

                    if (IdentificacionFiscalUtils.EsCif(valorIdentificacion))
                        return Result.Ok<SubTipoIdentificacion?>(SubTipoIdentificacion.NifPersonaJuridica);

                    return Result.Fail<SubTipoIdentificacion?>("No se ha podido determinar el subtipo de identificación.");

                default:
                    return Result.Ok<SubTipoIdentificacion?>(null);
            }
        }
    }
}
