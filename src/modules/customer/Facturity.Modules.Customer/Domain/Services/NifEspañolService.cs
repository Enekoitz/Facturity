namespace Facturity.Modules.Customer.Domain;

public static class NifEspañolService
{
    public static TiposNifEspañol ObtenerTipo(string valorIdentificacion)
    {
        if (IdentificacionFiscalValidaciones.EsDni(valorIdentificacion))
            return TiposNifEspañol.Dni;

        if (IdentificacionFiscalValidaciones.EsNie(valorIdentificacion))
            return TiposNifEspañol.Nie;

        if (IdentificacionFiscalValidaciones.EsCif(valorIdentificacion))
            return TiposNifEspañol.Cif;

        throw new ArgumentOutOfRangeException(nameof(valorIdentificacion));
    }
}