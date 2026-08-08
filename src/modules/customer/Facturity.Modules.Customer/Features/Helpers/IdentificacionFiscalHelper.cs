using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Facturity.Modules.Customer.Features;

public static class IdentificacionFiscalHelper
{
    public static TiposIdentificacionFiscal? ObtenerTipoIdentificacionFiscal(
        string? nif,
        string? idOtroExtranjero,
        TiposIdsOtrosExtranjeros? idOtroExtranjeroTipo,
        string? idOtroExtranjeroCodigoPais) => (nif, idOtroExtranjero, idOtroExtranjeroTipo, idOtroExtranjeroCodigoPais) 
    switch
    {
        (not null, null, null, null) when !string.IsNullOrWhiteSpace(nif)
            => TiposIdentificacionFiscal.NifEspañol,

        (null, not null, not null, not null) when !string.IsNullOrWhiteSpace(idOtroExtranjero) && !string.IsNullOrWhiteSpace(idOtroExtranjeroCodigoPais)
            => TiposIdentificacionFiscal.IdOtroExtranjero,

        _ => null
    };
}