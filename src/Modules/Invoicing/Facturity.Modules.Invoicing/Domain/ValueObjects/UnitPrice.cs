using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal record UnitPrice
{
    internal const int MaxDecimalPlaces = 6;
    
    internal decimal Price { get; private init; }

    internal UnitPrice(decimal price)
    {
        Price = price;
    }
    
    private UnitPrice(){}

    internal static Result<UnitPrice> Create(decimal price)
    {
        var errors = new List<Error>();
        
        if (price <= 0) 
            errors.Add(UnitPriceError.InvalidAmountNegativeOrZero);
        
        if (Utilities.GetSignificantDecimalPlaces(price) > MaxDecimalPlaces)
            errors.Add(UnitPriceError.ExceededMaximumDecimalPlaces);
        
        if (errors.Count > 0)
            return Result.Fail<UnitPrice>(errors);

        return Result.Ok(new UnitPrice(price));
    }
    
    public override string ToString() => $"{Price}";
}