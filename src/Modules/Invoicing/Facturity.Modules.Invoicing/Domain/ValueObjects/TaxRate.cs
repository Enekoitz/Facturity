using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal record TaxRate
{
    internal decimal Percentage { get; private init; }

    private TaxRate(decimal percentage)
    {
        Percentage = percentage;
    }
    
    private TaxRate(){}

    internal static Result<TaxRate> Create(decimal percentage)
    {
        if (percentage is < 0 or > 100) 
            return Result.Fail<TaxRate>(TaxRateError.InvalidTaxRate);
        
        return Result.Ok(new TaxRate(InvoiceRounding.Round(percentage)));
    }
    
    public override string ToString() => $"{Percentage}%";
}