namespace UgeFactura.Modules.Clientes.Domain;

public enum SubtiposIdentificacionFiscalReglas
{
    DNI,
    Nie,
    Cif
}

public static class SubtipoIdentificacionFiscalService
{
    public static SubtiposIdentificacionFiscalReglas? ObtenerSubtipo(
        IdentificacionFiscalReglasEnum IdentificacionFiscalReglas, 
        string valorIdentificacion) 
    {
        switch (IdentificacionFiscalReglas)
        {
            case IdentificacionFiscalReglasEnum.Nif:
                if (IdentificacionFiscalValidaciones.EsDNI(valorIdentificacion))
                    return SubtiposIdentificacionFiscalReglas.DNI;

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