namespace UgeFactura.Modules.Clientes.Domain
{
    internal enum ClienteCodigosError
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

    internal enum IdentificacionFiscalCodigosError
    {
        TipoIdentificacionFiscal_Vacio,
        ValorIdentificacionFiscal_Vacio,
        ValorIdentificacionFiscal_LongitudInvalida,
        SubTipoIdentificacionFiscal_Invalido,
        CodigoPais_Vacio,
        CodigoPais_LongitudInvalida,
        IdentificacionFiscalReglas_Desconocido
    }

    internal enum NIF_NIE_CIFCodigosError
    {
        NIF_NIE_CIF_Vacio,
        NIF_NIE_CIF_LongitudInvalida,
        NIF_NIE_CIF_FormatoInvalido,
    }

    internal enum NIFCodigosError
    {
        NIF_Vacio,
        NIF_PrimerosCaracteresInvalidos,
        NIF_LongitudInvalida,
        NIF_FormatoInvalido,
        NIF_LetraIncorrecta,
    }

    internal enum NIECodigosError
    {
        NIE_Vacio,
        NIE_PrimeraLetraInvalida,
        NIE_LongitudInvalida,
        NIE_FormatoInvalido,
        NIE_UltimaLetraInvalida,
    }

    internal enum CIFCodigosError
    {
        CIF_Vacio,
        CIF_LongitudInvalida,
        CIF_FormatoInvalido,
        CIF_PrimeraLetraInvalida,
    }

    internal enum OtrosTiposIdentificacionCodigosError
    {
        NIFEntidadExtranjera_Vacio,
        NIFEntidadExtranjera_LongitudInvalida,
        NIFEntidadExtranjera_FormatoInvalido,
        NIFEntidadExtranjera_PrimerosCaracteresInvalidos,
        NIFEntidadExtranjera_PrimeraLetraInvalida,
    }

    internal enum DomicilioCodigosError
    {
        Direccion_Vacio,
        CodigoPostal_Vacio,
        Municipio_Vacio,
        Provincia_Vacio,
        Region_Vacio,
        Pais_Vacio,
    }

    internal enum CodigoPostalCodigosError
    {
        CodigoPostal_Vacio,
        CodigoPostal_LongitudInvalida,
        CodigoPostal_FormatoInvalido,
    }
}
