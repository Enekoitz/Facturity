using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class InvoiceRoundingTests
{
    [Theory]
    [InlineData(21.345, 21.35)]
    [InlineData(21, 21)]
    [InlineData(21.343, 21.34)]
    [InlineData(21.346, 21.35)]
    [InlineData(-21.345, -21.35)]
    [InlineData(-21, -21)]
    [InlineData(-21.343, -21.34)]
    [InlineData(-21.346, -21.35)]
    public void InvoiceRounding_VariedDecimalsInput_ShouldRoundValuesApplyingAwayFromZeroRounding(
        decimal inputValue, 
        decimal expectedValue)
    {
        var roundedValue = InvoiceRounding.Round(inputValue);
        Assert.Equal(roundedValue, expectedValue);
    }
}