namespace Facturity.Modules.Customer.Domain;

internal static class TaxIdMapper
{
    internal static (string Value, TaxIdType Type, string CountryCode) GetCommonValues(this ITaxId taxId)
    {
        var (value, tipo, countryCode) = taxId switch
        {
            SpanishTaxId id => (id.Valor, TaxIdType.TaxId, "ES"),
            OtherTaxId id => (id.Value, id.Type, id.CountryCode),
            _ => throw new InvalidOperationException($"Tax ID is not supported: {taxId?.GetType().Name}")
        };
        
        return  (value, tipo, countryCode);
    }
}