using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class TaxRateTests
{
    [Theory]
    [InlineData(21.345)]
    [InlineData(21)]
    [InlineData(5.553)]
    [InlineData(0)]
    [InlineData(100)]
    public void Create_ValidPercentage_ReturnsSuccessTaxRateResult(decimal percentage)
    {
        var result = TaxRate.Create(percentage);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(InvoiceRounding.Round(percentage), result.Value.Percentage);
        Assert.Empty(result.Errors);
    }
    
    [Theory]
    [InlineData(-21.345)]
    [InlineData(210)]
    [InlineData(-0.000000001)]
    [InlineData(100.000000001)]
    public void Create_InvalidPercentage_ReturnsFailureResult(decimal percentage)
    {
        var result = TaxRate.Create(percentage);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Contains(TaxRateError.InvalidTaxRate, result.Errors);
    }
}