using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class InvoiceSeriesTests
{
    [Fact]
    public void Create_WithValidData_ShouldSuccessReturningInvoiceSeries()
    {
        var name = Utilities.GetRandomLength(InvoiceSeries.NameMaxLength);
        
        var result = Build.CreateSeries(name);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_ShouldKeepInputValues()
    {
        var result = Build.CreateLine(description: "Consulting", units: 2.5m, unitPrice: 10m, taxRate: 10m);

        Assert.Equal("Consulting", result.Value.Description);
        Assert.Equal(2.5m, result.Value.Units);
        Assert.Equal(10m, result.Value.UnitPrice.Price);
        Assert.Equal(10m, result.Value.TaxRate.Percentage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrBlankName_ShouldFail(string? name)
    {
        var result = Build.CreateSeries(name);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceSeriesErrors.NullOrEmptyName, result.Errors);
        Assert.DoesNotContain(InvoiceSeriesErrors.ExceededMaximumLengthName, result.Errors);
    }

    [Fact]
    public void Create_WithNameExceededMaxLength_ShouldFail()
    {
        var name = new string('a', InvoiceSeries.NameMaxLength + 1);

        var result = Build.CreateSeries(name);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceSeriesErrors.ExceededMaximumLengthName, result.Errors);
        Assert.DoesNotContain(InvoiceSeriesErrors.NullOrEmptyName, result.Errors);
    }

    [Fact]
    public void Create_ShouldTrimName()
    {
        var result = Build.CreateSeries("  DFB_MLC     ");

        Assert.Equal("DFB_MLC", result.Value.Name);
    }
}