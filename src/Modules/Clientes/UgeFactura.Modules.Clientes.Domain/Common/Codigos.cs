namespace UgeFactura.Modules.Clientes.Domain;

public enum ClienteCodigosError
{
    Id_Vacio,
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
    IdentificacionFiscalReglas_Desconocido
}

public enum NIFCodigosError
{
    NIF_Vacio,
    NIF_LongitudInvalida,
    NIF_FormatoInvalido,
}

public enum DNICodigosError
{
    DNI_Vacio,
    DNI_PrimerosCaracteresInvalidos,
    DNI_LongitudInvalida,
    DNI_FormatoInvalido,
    DNI_LetraIncorrecta,
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

public enum DomicilioCodigosError
{
    Direccion_Vacio,
    CodigoPostal_Vacio,
    Municipio_Vacio,
    Provincia_Vacio,
    Region_Vacio,
    Pais_Vacio,
}

public enum CodigoPostalCodigosError
{
    CodigoPostal_Vacio,
    CodigoPostal_LongitudInvalida,
    CodigoPostal_FormatoInvalido,
}