using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class UnitPriceTests
{
    [Theory]
    [InlineData(145.34)]
    [InlineData(43)]
    [InlineData(13454234231423423.45)]
    [InlineData(1345.45)]
    public void Create_PositiveAmount_ReturnsSuccessUnitPriceResult(decimal amount)
    {
        var result = UnitPrice.Create(amount);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(amount, result.Value.Price);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-134.52)]
    [InlineData(-134.52)]
    [InlineData(0)]
    [InlineData(null)]
    public void Create_NegativeOrZeroAmountAndInvalidCurrency_ReturnsFailureResult(decimal amount)
    {
        
        var result = UnitPrice.Create(amount);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Contains(UnitPriceError.InvalidAmountNegativeOrZero ,result.Errors);
    }
}