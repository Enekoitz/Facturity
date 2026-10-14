using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

/// <summary>
/// Money extension methods
/// </summary>
internal static class MoneyExtensions
{
    public static Money Sum<T>(this IEnumerable<T> source, Func<T, Money> selector) =>
        source.Aggregate(Money.Zero, (acc, item) => acc + selector(item));
}

/// <summary>
/// Money value object
/// </summary>
internal sealed record Money
{
    internal static readonly Money Zero = new(0m);
    
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
    
    public static Money operator +(Money left, Money right) =>
        new(left.Amount + right.Amount);
    
    public override string ToString() => $"{Amount:F2} {Currency}";
}
