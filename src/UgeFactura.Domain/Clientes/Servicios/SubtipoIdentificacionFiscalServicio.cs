using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using UgeFactura.Domain.Comun;

namespace UgeFactura.Domain.Clientes.Servicios
{
    public enum SubtiposIdentificacionFiscalRegla
    {
        Nif,
        Nie,
        Cif
    }
    public static class SubtipoIdentificacionFiscalServicio
    {
        public static SubtiposIdentificacionFiscalRegla? ObtenerSubtipo(ReglaIdentificacionFiscal reglaIdentificacionFiscal, string valorIdentificacion) {
            switch (reglaIdentificacionFiscal)
            {
                case ReglaIdentificacionFiscal.NifNieCif:
                    if (ValidarIdentificacionFiscalUtils.EsNif(valorIdentificacion))
                        return SubtiposIdentificacionFiscalRegla.Nif;

                    if (ValidarIdentificacionFiscalUtils.EsNie(valorIdentificacion))
                        return SubtiposIdentificacionFiscalRegla.Nie;

                    if (ValidarIdentificacionFiscalUtils.EsCif(valorIdentificacion))
                        return SubtiposIdentificacionFiscalRegla.Cif;

                    return null;

                default:
                    return null;
            }
        }
    }
}