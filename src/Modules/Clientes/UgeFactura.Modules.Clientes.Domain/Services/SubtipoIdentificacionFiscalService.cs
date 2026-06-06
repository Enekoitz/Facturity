namespace UgeFactura.Modules.Clientes.Domain
{
    public enum SubtiposIdentificacionFiscalReglas
    {
        Nif,
        Nie,
        Cif
    }

    public static class SubtipoIdentificacionFiscalService
    {
        public static SubtiposIdentificacionFiscalReglas? ObtenerSubtipo(IdentificacionFiscalReglasEnum IdentificacionFiscalReglas, string valorIdentificacion) {
            switch (IdentificacionFiscalReglas)
            {
                case IdentificacionFiscalReglasEnum.NifNieCif:
                    if (IdentificacionFiscalValidaciones.EsNif(valorIdentificacion))
                        return SubtiposIdentificacionFiscalReglas.Nif;

                    if (IdentificacionFiscalValidaciones.EsNie(valorIdentificacion))
                        return SubtiposIdentificacionFiscalReglas.Nie;

                    if (IdentificacionFiscalValidaciones.EsCif(valorIdentificacion))
                        return SubtiposIdentificacionFiscalReglas.Cif;

                    return null;

                default:
                    return null;
            }
        }
    }
}