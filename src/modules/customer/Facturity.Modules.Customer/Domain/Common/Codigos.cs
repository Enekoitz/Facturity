namespace Facturity.Modules.Customer.Domain;

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

public enum NifCodigosError
{
    Nif_Vacio,
    Nif_LongitudInvalida,
    Nif_FormatoInvalido,
}

public enum DniCodigosError
{
    Dni_Vacio,
    Dni_PrimerosCaracteresInvalidos,
    Dni_LongitudInvalida,
    Dni_FormatoInvalido,
    Dni_LetraIncorrecta,
}

public enum NieCodigosError
{
    Nie_Vacio,
    Nie_PrimeraLetraInvalida,
    Nie_LongitudInvalida,
    Nie_FormatoInvalido,
    Nie_UltimaLetraInvalida,
}

public enum CifCodigosError
{
    Cif_Vacio,
    Cif_LongitudInvalida,
    Cif_FormatoInvalido,
    Cif_PrimeraLetraInvalida,
}

public enum IdOtroExtranjeroCodigosError
{
    IdOtroExtranjero_IdVacio,
    IdOtroExtranjero_LongitudInvalida,
    IdOtroExtranjero_FormatoInvalido,
    IdOtroExtranjero_CaracteresInvalidos,
    IdOtroExtranjero_CodigoPaisES,
    IdOtroExtranjero_CodigoPaisVacio,
    IdOtroExtranjero_CodigoPaisLongitudNoISO2,
    IdOtroExtranjero_NifIva_Invalido,
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