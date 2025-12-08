using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Cliente.Servicios
{
    public class TiposIdentificacion
    {
        private static readonly Dictionary<string, ValueObjects.TipoIdentificacion> _tipos = new()
        {
            { "01", new ValueObjects.TipoIdentificacion("01", "NIF", (valor) => (IdentificacionFiscalUtils.EsNifNieOCifValido(valor, out string msj), msj), false) },
            { "02", new ValueObjects.TipoIdentificacion("02", "NIF-IVA", (valor) => (IdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "NIF-IVA", out string msj), msj), true) },
            { "03", new ValueObjects.TipoIdentificacion("03", "Pasaporte", (valor) => (IdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "Pasaporte", out string msj), msj), true) },
            { "04", new ValueObjects.TipoIdentificacion("04", "Documento oficial de identificación expedido por el país o territorio de residencia", (valor) => (IdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "Documento oficial de identificación expedido por el país o territorio de residencia", out string msj), msj), true) },
            { "05", new ValueObjects.TipoIdentificacion("05", "Certificado de residencia", (valor) => (IdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "Certificado de residencia", out string msj), msj), true) },
            { "06", new ValueObjects.TipoIdentificacion("06", "Otro documento probatorio", (valor) => (IdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "Otro documento probatorio", out string msj), msj), true) },
            { "07", new ValueObjects.TipoIdentificacion("07", "NIF Entidad extranjera" , (valor) => (IdentificacionFiscalUtils.EsNifEntidadExtranjeraValido(valor, out string msj), msj), true) },
        };

        private static readonly Dictionary<string, string[]> _subtipoSegunTipos = new()
        {
            { "01", new string[] { "NIF", "NIE", "CIF" } },
            { "02", new string[] { "NIF-IVA" } },
            { "03", new string[] { "Pasaporte" } },
            { "04", new string[] { "Documento oficial de identificación expedido por el país o territorio de residencia" } },
            { "05", new string[] { "Certificado de residencia" } },
            { "06", new string[] { "Otro documento probatorio" } },
            { "07", new string[] { "NIF Entidad extranjera" } },
        };

        public static Result<ValueObjects.TipoIdentificacion> Obtener(string tipoCodigo)
        {
            return _tipos.TryGetValue(tipoCodigo, out ValueObjects.TipoIdentificacion? value)
                ? Result.Ok(value)
                : Result.Fail<ValueObjects.TipoIdentificacion>("El código de tipo de identificación no es válido.");
        }

        public static string? ObtenerSubtipo(string tipoCodigo, string valor)
        {
            if (_subtipoSegunTipos.TryGetValue(tipoCodigo, out string[] subtipos))
            {
                if (tipoCodigo == "01")
                {
                    if (IdentificacionFiscalUtils.EsNifValido(valor, out _))
                        return "NIF";
                    else if (IdentificacionFiscalUtils.EsNieValido(valor, out _))
                        return "NIE";
                    else if (IdentificacionFiscalUtils.EsCifValido(valor, out _))
                        return "CIF";
                    else
                        return null;
                }
                else
                {
                    return subtipos.FirstOrDefault() ?? null;
                }
            }
            return null;
        }
    }
}
