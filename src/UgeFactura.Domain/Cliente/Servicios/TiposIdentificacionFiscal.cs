using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun;
using UgeFactura.Domain.Comun.Validaciones;

namespace UgeFactura.Domain.Cliente.Servicios
{
    public class TiposIdentificacionFiscal
    {
        private static readonly Dictionary<string, ValueObjects.TipoIdentificacionFiscal> _tipos = new()
        {
            { "01", new ValueObjects.TipoIdentificacionFiscal("01", "NIF", (valor) => (ValidarIdentificacionFiscalUtils.EsNifNieOCifValido(valor, out string msj), msj), false) },
            { "02", new ValueObjects.TipoIdentificacionFiscal("02", "NIF-IVA", (valor) => (ValidarIdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "NIF-IVA", out string msj), msj), true) },
            { "03", new ValueObjects.TipoIdentificacionFiscal("03", "Pasaporte", (valor) => (ValidarIdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "Pasaporte", out string msj), msj), true) },
            { "04", new ValueObjects.TipoIdentificacionFiscal("04", "Documento oficial de identificación expedido por el país o territorio de residencia", (valor) => (ValidarIdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "Documento oficial de identificación expedido por el país o territorio de residencia", out string msj), msj), true) },
            { "05", new ValueObjects.TipoIdentificacionFiscal("05", "Certificado de residencia", (valor) => (ValidarIdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "Certificado de residencia", out string msj), msj), true) },
            { "06", new ValueObjects.TipoIdentificacionFiscal("06", "Otro documento probatorio", (valor) => (ValidarIdentificacionFiscalUtils.EsIdentificacionNoNifValida(valor, "Otro documento probatorio", out string msj), msj), true) },
            { "07", new ValueObjects.TipoIdentificacionFiscal("07", "NIF Entidad extranjera" , (valor) => (ValidarIdentificacionFiscalUtils.EsNifEntidadExtranjeraValido(valor, out string msj), msj), true) },
        };

        public static Result<ValueObjects.TipoIdentificacionFiscal> Obtener(string tipoCodigo)
        {
            if (string.IsNullOrWhiteSpace(tipoCodigo))
                return Result.Fail<ValueObjects.TipoIdentificacionFiscal>("El código de tipo de identificación no es válido.");

            return _tipos.TryGetValue(tipoCodigo, out ValueObjects.TipoIdentificacionFiscal? value)
                ? Result.Ok(value)
                : Result.Fail<ValueObjects.TipoIdentificacionFiscal>("El código de tipo de identificación no es válido.");
        }
        
    }
}
