using System.Diagnostics;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal record Money
{
    internal const string DefaultCurrency = "EUR";
    internal decimal Amount { get; private init; }
    internal string Currency { get; private init; } = DefaultCurrency;

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    private Money(decimal amount)
    {
        Amount = amount;
    }
    
    private Money(){}

    public static Result<Money> Create(decimal amount, string currency = DefaultCurrency)
    {
        var errors = new List<Error>();
        
        if (amount <= 0) 
            errors.Add(MoneyError.InvalidAmountNegativeOrZero);
        
        currency ??= DefaultCurrency;
        
        currency = currency.Trim().ToUpper();
        if (currency.Length != 3)
            errors.Add(MoneyError.InvalidCurrencyCode);
        
        if (errors.Count > 0)
            return Result.Fail<Money>(errors);

        return Result.Ok(new Money(amount, currency));
    }
    
    public override string ToString() => $"{Amount} {Currency}";
}