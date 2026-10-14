using Facturity.Modules.Invoicing.UnitTests.Domain;

namespace Facturity.Modules.Invoicing.Domain;

public class MoneyExtensionsTests
{
    private record Item(Money Amount);

    [Fact]
    public void Sum_WithEmptyCollection_ShouldReturnZero()
    {
        var items = Array.Empty<Item>();

        var result = items.Sum(i => i.Amount);

        Assert.Equal(Money.Zero, result);
    }

    [Fact]
    public void Sum_WithSingleItem_ShouldReturnThatAmount()
    {
        var items = new[] { new Item(Helper.GetMoney(10.50m)) };

        Assert.Equal(Helper.GetMoney(10.50m), items.Sum(i => i.Amount));
    }

    [Fact]
    public void Sum_WithSeveralItems_ShouldReturnTotal()
    {
        var items = new[]
        {
            new Item(Helper.GetMoney(10.50m)),
            new Item(Helper.GetMoney(0.25m)),
            new Item(Helper.GetMoney(5.00m))
        };

        Assert.Equal(Helper.GetMoney(15.75m), items.Sum(i => i.Amount));
    }

    [Fact]
    public void Sum_ShouldUseTheSelector()
    {
        var items = new[] { (A: Helper.GetMoney(1m), B: Helper.GetMoney(100m)), (A: Helper.GetMoney(2m), B: Helper.GetMoney(200m)) };

        Assert.Equal(Helper.GetMoney(3m), items.Sum(i => i.A));
        Assert.Equal(Helper.GetMoney(300m), items.Sum(i => i.B));
    }

    [Fact]
    public void Sum_ShouldNotDependOnOrder()
    {
        var a = new[] { new Item(Helper.GetMoney(1.10m)), new Item(Helper.GetMoney(2.20m)), new Item(Helper.GetMoney(3.30m)) };
        var b = a.Reverse().ToArray();

        Assert.Equal(a.Sum(i => i.Amount), b.Sum(i => i.Amount));
    }
}

public class MoneyTests
{
    [Theory]
    [InlineData(145.34, "EUR")]
    [InlineData(43, "HKD")]
    [InlineData(13454234231423423.45, "NOK")]
    [InlineData(1345.45, "USD")]
    public void Create_PositiveAmountAndValidCurrency_ReturnsSuccessMoneyResult(decimal amount, string currency)
    {
        var result = Money.Create(amount, currency);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(amount, result.Value.Amount);
        Assert.Equal(currency.Trim().ToUpper(), result.Value.Currency);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public void Create_PositiveAmountAndNullCurrency_ReturnsSuccessMoneyResult()
    {
        const decimal amount = 1234.53m;
        const string? currency = null; 
        var result = Money.Create(amount, currency);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(amount, result.Value.Amount);
        Assert.Equal(Money.DefaultCurrency, result.Value.Currency);
        Assert.Empty(result.Errors);
    }
    
    [Theory]
    [InlineData(145.34)]
    [InlineData(43)]
    [InlineData(1345.45)]
    [InlineData(000434342341345.45)]
    public void Create_PositiveAmountAndNoCurrency_ReturnsSuccessMoneyResult(decimal amount)
    {
        var result = Money.Create(amount);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(amount, result.Value.Amount);
        Assert.Equal(Money.DefaultCurrency, result.Value.Currency);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(-124.63)]
    [InlineData(0)]
    public void Create_NegativeOrZeroAmountAndValidCurrencyCode_ReturnsFailureResult(decimal amount)
    {
        const string currency = "EUR";
        
        var result = Money.Create(amount, currency);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(MoneyError.InvalidAmountNegativeOrZero ,result.Errors[0]);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("EU")]
    [InlineData("EURRRRRRRRR")]
    public void Create_InvalidCurrencyAndValidAmount_ReturnsFailureResult(string currency)
    {
        const decimal amount = 12.63m;
        
        var result = Money.Create(amount, currency);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(MoneyError.InvalidCurrencyCode ,result.Errors[0]);
    }
    
    [Theory]
    [InlineData(0, "")]
    [InlineData(-134.52, "   ")]
    [InlineData(-134.52, "EU")]
    [InlineData(0, "EURRRRRRRRR")]
    [InlineData(null, "EURRRRRRRRR")]
    public void Create_NegativeOrZeroAmountAndInvalidCurrency_ReturnsFailureResult(decimal amount, string currency)
    {
        
        var result = Money.Create(amount, currency);
        
        Assert.True(result.IsFailure);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(MoneyError.InvalidAmountNegativeOrZero ,result.Errors);
        Assert.Contains(MoneyError.InvalidCurrencyCode ,result.Errors);
    }
    
    [Theory]
    [InlineData(145.345, 12.455, 157.810000)]
    [InlineData(43.006, 17.001, 60.01000000)]
    [InlineData(134.45, 755.43, 889.88)]
    [InlineData(1345.45, 44.345, 1389.8)]
    public void SumOperation_ValidMoneyObjectsSum_ReturnsSuccessMoney(decimal firstMoneyAmount, decimal secondMoneyAmount, decimal expectedSum)
    {
        var firstMoney = Money.Create(firstMoneyAmount).Value;
        var secondMoney = Money.Create(secondMoneyAmount).Value;
        var expectedSumMoney = Money.Create(expectedSum).Value;
        
        var resultMoney = firstMoney + secondMoney;
        
        Assert.IsType<Money>(resultMoney);
        Assert.Equal(expectedSumMoney, resultMoney);
    }
}