using Facturity.Modules.Invoicing.UnitTests.Domain;

namespace Facturity.Modules.Invoicing.Domain;

public class InvoiceLineTests
{
    [Fact]
    public void Create_WithValidData_ShouldCalculateTaxBaseTaxAmountAndTotal()
    {
        var result = Helper.CreateLine(units: 2m, unitPrice: 10m, taxRate: 21m);

        Assert.True(result.IsSuccess);
        Assert.Equal(20.00m, result.Value.TaxBase.Amount);
        Assert.Equal(4.20m, result.Value.TaxAmount.Amount);
        Assert.Equal(24.20m, result.Value.TotalAmount.Amount);
    }

    [Theory]
    [InlineData(4.35, 12.4, 21, 53.94, 11.33, 65.27)]   
    [InlineData(1, 0.125, 21, 0.13, 0.03, 0.16)]
    [InlineData(1, 0.50, 21, 0.50, 0.11, 0.61)]
    [InlineData(1, 0.145, 4, 0.15, 0.01, 0.16)]
    public void Create_ShouldApplyAwayFromZeroRoundingInChain(
        decimal units, decimal unitPrice, decimal taxRate,
        decimal expectedBase, decimal expectedTax, decimal expectedTotal)
    {
        var result = Helper.CreateLine(units: units, unitPrice: unitPrice, taxRate: taxRate);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedBase, result.Value.TaxBase.Amount);
        Assert.Equal(expectedTax, result.Value.TaxAmount.Amount);
        Assert.Equal(expectedTotal, result.Value.TotalAmount.Amount);
    }

    [Fact]
    public void Create_ShouldKeepInputValues()
    {
        var result = Helper.CreateLine(description: "Consulting", units: 2.5m, unitPrice: 10m, taxRate: 10m);

        Assert.Equal("Consulting", result.Value.Description);
        Assert.Equal(2.5m, result.Value.Units);
        Assert.Equal(10m, result.Value.UnitPrice.Price);
        Assert.Equal(10m, result.Value.TaxRate.Percentage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrBlankDescription_ShouldFailWithoutThrowing(string? description)
    {
        var result = Helper.CreateLine(description: description);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceLineError.NullOrEmptyDescription, result.Errors);
        Assert.DoesNotContain(InvoiceLineError.ExceededMaximumLengthDescription, result.Errors);
    }

    [Fact]
    public void Create_WithDescriptionOfMaxLength_ShouldSucceed()
    {
        var description = new string('a', Parameters.DescriptionMaxLength);

        var result = Helper.CreateLine(description: description);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_WithDescriptionOverMaxLength_ShouldFail()
    {
        var description = new string('a', Parameters.DescriptionMaxLength + 1);

        var result = Helper.CreateLine(description: description);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceLineError.ExceededMaximumLengthDescription, result.Errors);
    }

    [Fact]
    public void Create_ShouldTrimDescription()
    {
        var result = Helper.CreateLine(description: "  Consulting  ");

        Assert.Equal("Consulting", result.Value.Description);
    }

    [Fact]
    public void Create_WithMaxLengthDescriptionPlusSurroundingSpaces_ShouldSucceed()
    {
        var description = "  " + new string('a', Parameters.DescriptionMaxLength) + "  ";

        var result = Helper.CreateLine(description: description);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithZeroOrNegativeUnits_ShouldFail(decimal units)
    {
        var result = Helper.CreateLine(units: units);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceLineError.NegativeOrZeroUnits, result.Errors);
    }

    [Theory]
    [InlineData(0.25)]
    [InlineData(1.50)]
    [InlineData(1.500000)]
    public void Create_WithUnitsWithinMaxDecimals_ShouldSucceed(decimal units)
    {
        var result = Helper.CreateLine(units: units);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(0.125)]
    [InlineData(1.001)]
    public void Create_WithUnitsOverMaxDecimals_ShouldFailAndNotRound(decimal units)
    {
        var result = Helper.CreateLine(units: units);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceLineError.ExceededMaximumDecimalPlacesUnits, result.Errors);
    }

    [Fact]
    public void Create_WithSeveralInvalidFields_ShouldReturnAllErrors()
    {
        var result = Helper.CreateLine(description: "", units: 0m);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceLineError.NullOrEmptyDescription, result.Errors);
        Assert.Contains(InvoiceLineError.NegativeOrZeroUnits, result.Errors);
    }
}