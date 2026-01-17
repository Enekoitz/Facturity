using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Clientes
{
    public enum ReglaIdentificacionFiscal
    { 
        NifNieCif,
        OtroDocumento,
        NifEntidadExtranjera,
        Desconocido
    }

    public static class IdentificacionFiscalReglas
    {
        public static Func<string, (bool, string)> Obtener(ReglaIdentificacionFiscal regla)
        => regla switch
        {
            ReglaIdentificacionFiscal.NifNieCif =>
                valor => (ValidarIdentificacionFiscalUtils.EsNifNieOCifValido(valor, out var msj), msj),

            ReglaIdentificacionFiscal.OtroDocumento =>
                valor => (ValidarIdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, out var msj), msj),

            ReglaIdentificacionFiscal.NifEntidadExtranjera =>
                valor => (ValidarIdentificacionFiscalUtils.EsNifEntidadExtranjeraValido(valor, out var msj), msj),

            ReglaIdentificacionFiscal.Desconocido => 
                valor => (false, IdentificacionFiscalCodigosError.ReglaIdentificacionFiscal_Desconocido.ToString()),
            
            _ =>
                valor => (false, String.Empty)
        };

    }
}
