using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

/// <summary>
/// Money extension methods
/// </summary>
internal static class MoneyExtension
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
    internal const int MaxDecimalPlaces = 2;
    
    internal decimal Amount { get; private init; }

    private Money(decimal amount)
    {
        Amount = amount;
    }
    
    private Money(){}

    public static Result<Money> Create(decimal amount)
    {
        var errors = new List<Error>();
        
        if (amount <= 0) 
            errors.Add(MoneyError.InvalidAmountNegativeOrZero);
        
        if (errors.Count > 0)
            return Result.Fail<Money>(errors);

        return Result.Ok(new Money(InvoiceRounding.Round(amount)));
    }
    
    public static Money operator +(Money left, Money right) =>
        new(left.Amount + right.Amount);
    
    public override string ToString() => $"{Amount:F2}";
}
