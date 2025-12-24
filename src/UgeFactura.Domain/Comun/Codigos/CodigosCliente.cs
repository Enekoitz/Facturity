using System;
using System.Collections.Generic;
using System.Text;

namespace UgeFactura.Domain.Comun.Mensajes
{
    public enum ClienteCodigosError
    {
        Alias_Vacio,
        Alias_DemasiadoLargo,
        Nombre_Vacio,
        PrimerApellido_Vacio,
        Nombre_DemasiadoLargo,
        PrimerApellido_DemasiadoLargo,
        SegundoApellido_DemasiadoLargo,
        RazonSocial_Vacio,
        RazonSocial_DemasiadoLargo,
    }

    public enum IdentificacionFiscalCodigosError
    {
        TipoIdentificacionFiscal_Vacio,
        ValorIdentificacionFiscal_Vacio,
        ValorIdentificacionFiscal_LongitudInvalida,
        SubTipoIdentificacionFiscal_Invalido,
        CodigoPais_Vacio,
        CodigoPais_LongitudInvalida,
    }

    public enum NIF_NIE_CIFCodigosError
    {
        NIF_NIE_CIF_Vacio,
        NIF_NIE_CIF_LongitudInvalida,
        NIF_NIE_CIF_FormatoInvalido,
    }

    public enum NIFCodigosError
    {
        NIF_Vacio,
        NIF_PrimerosCaracteresInvalidos,
        NIF_LongitudInvalida,
        NIF_FormatoInvalido,
        NIF_LetraIncorrecta,
    }

    public enum NIECodigosError
    {
        NIE_Vacio,
        NIE_PrimeraLetraInvalida,
        NIE_LongitudInvalida,
        NIE_FormatoInvalido,
        NIE_UltimaLetraInvalida,
    }

    public enum CIFCodigosError
    {
        CIF_Vacio,
        CIF_LongitudInvalida,
        CIF_FormatoInvalido,
        CIF_PrimeraLetraInvalida,
    }

    public enum OtrosTiposIdentificacionCodigosError
    {
        NIFEntidadExtranjera_Vacio,
        NIFEntidadExtranjera_LongitudInvalida,
        NIFEntidadExtranjera_FormatoInvalido,
        NIFEntidadExtranjera_PrimerosCaracteresInvalidos,
        NIFEntidadExtranjera_PrimeraLetraInvalida,
    }
}
