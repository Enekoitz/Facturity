using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class TaxEngineTests
{
    [Theory]
    [InlineData(4.35, 12.4, 53.94)]
    [InlineData(0, 12.4, 0)]
    [InlineData(34.345, 0.126, 4.33)]
    [InlineData(3, 0.125, 0.38)]
    [InlineData(1, 0.125, 0.13)]
    [InlineData(1, 0.145, 0.15)]
    [InlineData(-1, 0.125, -0.13)]
    public void CalculateTaxBase_ShouldReturnExpectedTaxBase(decimal units, decimal unitPrice, decimal expected)
    {
        var unitPriceResult = UnitPrice.Create(unitPrice);
        var result = TaxEngine.CalculateTaxBase(units, unitPriceResult.Value);
        
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(35.15, 21.00, 7.38)]
    public void CalculateTaxAmount_ShouldReturnExpectedTaxAmount(decimal taxBase, decimal taxRate, decimal expected)
    {
        var taxRateResult = TaxRate.Create(taxRate);
        var taxBaseResult = Money.Create(taxBase);
        
        var result = TaxEngine.CalculateTaxAmount(taxBaseResult.Value, taxRateResult.Value);
        
        Assert.Equal(expected, result);
    }
}