using System;
using System.Collections.Generic;
using System.Text;

namespace UgeFactura.Domain.Clientes.Servicios
{
    public class ReglaTipoIdentificacionHelper
    {
        public static class ReglaIdentificacionFiscalHelper
        {
            public static ReglaIdentificacionFiscal ParseOrDesconocido(string value)
            {
                if (Enum.TryParse<ReglaIdentificacionFiscal>(value, out var result))
                    return result;

                return ReglaIdentificacionFiscal.Desconocido;
            }
        }
    }
}
