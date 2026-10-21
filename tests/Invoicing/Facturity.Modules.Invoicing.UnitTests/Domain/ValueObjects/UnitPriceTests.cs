using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class UnitPriceTests
{
    [Theory]
    [InlineData(145.34, "EUR")]
    [InlineData(43, "HKD")]
    [InlineData(13454234231423423.45, "NOK")]
    [InlineData(1345.45, "USD")]
    public void Create_PositiveAmountAndValidCurrency_ReturnsSuccessUnitPriceResult(decimal amount, string currency)
    {
        var result = UnitPrice.Create(amount, currency);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(amount, result.Value.Price);
        Assert.Equal(currency.Trim().ToUpper(), result.Value.Currency);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public void Create_PositiveAmountAndNullCurrency_ReturnsSuccessUnitPriceResult()
    {
        const decimal amount = 1234.53m;
        const string? currency = null; 
        var result = UnitPrice.Create(amount, currency);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(amount, result.Value.Price);
        Assert.Equal(UnitPrice.DefaultCurrency, result.Value.Currency);
        Assert.Empty(result.Errors);
    }
    
    [Theory]
    [InlineData(145.34)]
    [InlineData(43)]
    [InlineData(1345.45)]
    [InlineData(000434342341345.45)]
    public void Create_PositiveAmountAndNoCurrency_ReturnsSuccessUnitPriceResult(decimal price)
    {
        var result = UnitPrice.Create(price);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(price, result.Value.Price);
        Assert.Equal(UnitPrice.DefaultCurrency, result.Value.Currency);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(-124.63)]
    [InlineData(0)]
    public void Create_NegativeOrZeroAmountAndValidCurrencyCode_ReturnsFailureResult(decimal amount)
    {
        const string currency = "EUR";
        
        var result = UnitPrice.Create(amount, currency);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(UnitPriceError.InvalidAmountNegativeOrZero ,result.Errors[0]);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("EU")]
    [InlineData("EURRRRRRRRR")]
    public void Create_InvalidCurrencyAndValidAmount_ReturnsFailureResult(string currency)
    {
        const decimal amount = 12.63m;
        
        var result = UnitPrice.Create(amount, currency);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(UnitPriceError.InvalidCurrencyCode ,result.Errors[0]);
    }
    
    [Theory]
    [InlineData(0, "")]
    [InlineData(-134.52, "   ")]
    [InlineData(-134.52, "EU")]
    [InlineData(0, "EURRRRRRRRR")]
    [InlineData(null, "EURRRRRRRRR")]
    public void Create_NegativeOrZeroAmountAndInvalidCurrency_ReturnsFailureResult(decimal amount, string currency)
    {
        
        var result = UnitPrice.Create(amount, currency);
        
        Assert.True(result.IsFailure);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(UnitPriceError.InvalidAmountNegativeOrZero ,result.Errors);
        Assert.Contains(UnitPriceError.InvalidCurrencyCode ,result.Errors);
    }
}