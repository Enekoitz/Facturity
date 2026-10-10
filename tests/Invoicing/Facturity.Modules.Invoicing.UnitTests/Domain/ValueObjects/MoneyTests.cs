namespace Facturity.Modules.Invoicing.Domain;

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
}