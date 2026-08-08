namespace Facturity.Modules.Customer.Domain;

public static class IdOtroExtranjeroService
{
    public static string AIdTypeTicketBai(this TiposIdsOtrosExtranjeros tipo) => tipo switch
    {
        TiposIdsOtrosExtranjeros.NifIvaComunitario => "02",
        TiposIdsOtrosExtranjeros.Pasaporte => "03",
        TiposIdsOtrosExtranjeros.DocumentoIdentidadPaisOrigen => "04",
        TiposIdsOtrosExtranjeros.CertificadoResidencia => "05",
        TiposIdsOtrosExtranjeros.OtroDocumentoProbatorio => "06",
        _ => throw new ArgumentOutOfRangeException(nameof(tipo))
    };
}