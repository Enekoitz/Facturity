using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class InvoiceSeriesTests
{
    [Theory]
    [InlineData("GENERATE_MAX_LENGTH_TEXT")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithValidData_ShouldSuccessReturningInvoiceSeries(string? description)
    {
        const string descriptionControlCode = "GENERATE_MAX_LENGTH_TEXT";
        
        var name = Utilities.GetRandomLength(InvoiceSeries.NameMaxLength);
        
        if (description == descriptionControlCode) 
            description = Utilities.GetRandomLength(InvoiceSeries.DescriptionMaxLength);
        
        var tenantId = Build.Tenant();
        var result = Build.CreateSeries(tenantId, name, description);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrBlankName_ShouldFail(string name)
    {
        var tenantId = Build.Tenant();
        var result = Build.CreateSeries(tenantId, name);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceSeriesErrors.NameEmpty, result.Errors);
        Assert.DoesNotContain(InvoiceSeriesErrors.NameTooLong, result.Errors);
    }

    [Fact]
    public void Create_WithValuesExceededMaxLength_ShouldFail()
    {
        var name = Utilities.GetRandomLength(InvoiceSeries.NameMaxLength + 1);
        var description = Utilities.GetRandomLength(InvoiceSeries.DescriptionMaxLength + 1);

        var tenantId = Build.Tenant();
        var result = Build.CreateSeries(tenantId, name, description);

        Assert.True(result.IsFailure);
        Assert.DoesNotContain(InvoiceSeriesErrors.NameEmpty, result.Errors);
        Assert.Contains(InvoiceSeriesErrors.NameTooLong, result.Errors);
        Assert.Contains(InvoiceSeriesErrors.DescriptionTooLong, result.Errors);
    }

    [Fact]
    public void Create_ShouldTrimNameAndDescription()
    {
        var tenantId = Build.Tenant();
        var result = Build.CreateSeries(
            tenantId
            ,"  DFB_MLC     "
            , "    Esta es una serie de MLC ejemplo TRIM   a           ");

        Assert.Equal("DFB_MLC", result.Value.Name);
        Assert.Equal("Esta es una serie de MLC ejemplo TRIM   a", result.Value.Description);
    }
}