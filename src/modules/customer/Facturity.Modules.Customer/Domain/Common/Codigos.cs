namespace Facturity.Modules.Customer.Domain;

public enum ClienteCodigosError
{
    IdVacio,
    AliasVacio,
    AliasDemasiadoLargo,
    NombreVacio,
    PrimerApellidoVacio,
    NombreDemasiadoLargo,
    PrimerApellidoDemasiadoLargo,
    SegundoApellidoDemasiadoLargo,
    RazonSocialVacio,
    RazonSocialDemasiadoLargo,
}

public enum NifCodigosError
{
    NifVacio,
    NifLongitudInvalida,
    NifFormatoInvalido,
}

public enum DniCodigosError
{
    DniVacio,
    DniPrimerosCaracteresInvalidos,
    DniLongitudInvalida,
    DniFormatoInvalido,
    DniLetraIncorrecta,
}

public enum NieCodigosError
{
    NieVacio,
    NiePrimeraLetraInvalida,
    NieLongitudInvalida,
    NieFormatoInvalido,
    NieUltimaLetraInvalida,
}

public enum CifCodigosError
{
    CifVacio,
    CifLongitudInvalida,
    CifFormatoInvalido,
    CifPrimeraLetraInvalida,
}

public enum IdOtroExtranjeroCodigosError
{
    IdOtroExtranjeroInvalido,
    IdOtroExtranjeroIdVacio,
    IdOtroExtranjeroLongitudInvalida,
    IdOtroExtranjeroFormatoInvalido,
    IdOtroExtranjeroCaracteresInvalidos,
    IdOtroExtranjeroCodigoPaisEs,
    IdOtroExtranjeroCodigoPaisVacio,
    IdOtroExtranjeroCodigoPaisLongitudNoIso2,
    IdOtroExtranjeroNifIvaInvalido,
}

public enum DomicilioCodigosError
{
    DireccionVacio,
    CodigoPostalVacio,
    MunicipioVacio,
    ProvinciaVacio,
    RegionVacio,
    PaisVacio,
}

public enum CodigoPostalCodigosError
{
    CodigoPostalVacio,
    CodigoPostalLongitudInvalida,
    CodigoPostalFormatoInvalido,
}