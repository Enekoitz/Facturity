using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class InvoiceHeaderTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_WithValidData_ShouldSuccessReturningInvoiceHeader(bool isSimplified)
    {
        var series = Build.CreateSeries().Value;
        
        var result = Build.Header(series, isSimplified);

        Assert.True(result.IsSuccess);
        Assert.Equal(series, result.Value.Series);
        Assert.Equal(isSimplified, result.Value.IsSimplified);
    }

    [Fact]
    public void Create_WithoutSeries_ShouldFail()
    {
        InvoiceSeries series = null; 
        const bool isSimplified = false;
        DateTimeOffset? issuedAtUtc = null;
        
        var result = Build.Header(series, isSimplified);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceHeaderError.SeriesRequired, result.Errors);
    }

    [Fact]
    public void WithIssuance_ShouldSuccess()
    {
        var seriesId = Guid.NewGuid();
        const bool isSimplified = false;
        var series = Build.CreateSeries().Value;
        
        var issuedAtUtc = new DateTimeOffset(2026, 12, 29, 09, 00,00, TimeSpan.Zero);
        
        var header = Build.Header(series, isSimplified).Value;
        
        header = header.WithIssuance(issuedAtUtc);

        Assert.Equal(header.IssuedAtUtc, issuedAtUtc);
    }
}