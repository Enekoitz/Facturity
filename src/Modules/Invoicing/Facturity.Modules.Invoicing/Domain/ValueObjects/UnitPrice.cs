using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal record UnitPrice
{
    internal const string DefaultCurrency = "EUR";
    internal decimal Price { get; private init; }
    internal string Currency { get; private init; } = DefaultCurrency;

    internal UnitPrice(decimal price, string currency)
    {
        Price = price;
        Currency = currency;
    }
    
    private UnitPrice(){}

    internal static Result<UnitPrice> Create(decimal price, string? currency = DefaultCurrency)
    {
        var errors = new List<Error>();
        
        if (price <= 0) 
            errors.Add(UnitPriceError.InvalidAmountNegativeOrZero);
        
        currency ??= DefaultCurrency;
        
        currency = currency.Trim().ToUpper();
        if (currency.Length != Parameters.CurrencyExactLength)
            errors.Add(UnitPriceError.InvalidCurrencyCode);

        if (Utilities.GetSignificantDecimalPlaces(price) > Parameters.UnitPriceMaxDecimalPlaces)
            errors.Add(UnitPriceError.ExceededMaximumDecimalPlaces);
        
        if (errors.Count > 0)
            return Result.Fail<UnitPrice>(errors);

        return Result.Ok(new UnitPrice(price, currency));
    }
    
    public override string ToString() => $"{Price} {Currency}";
}