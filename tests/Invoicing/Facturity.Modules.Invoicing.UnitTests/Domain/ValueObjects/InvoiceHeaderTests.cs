using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class InvoiceHeaderTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_WithValidData_ShouldSuccessReturningInvoiceHeader(bool isSimplified)
    {
        var seriesId = Guid.NewGuid(); 
        
        var result = Build.Header(seriesId, isSimplified);

        Assert.True(result.IsSuccess);
        Assert.Equal(seriesId, result.Value.SeriesId);
        Assert.Equal(isSimplified, result.Value.IsSimplified);
    }

    [Fact]
    public void Create_WithoutSeriesId_ShouldFail()
    {
        var seriesId = Guid.Empty;
        const bool isSimplified = false;
        DateTimeOffset? issuedAtUtc = null;
        
        var result = Build.Header(seriesId, isSimplified);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceHeaderError.SeriesIdEmpty, result.Errors);
    }

    [Fact]
    public void WithIssuance_ShouldSuccess()
    {
        var seriesId = Guid.NewGuid();
        const bool isSimplified = false;
        
        var issuedAtUtc = new DateTimeOffset(2026, 12, 29, 09, 00,00, TimeSpan.Zero);
        
        var header = Build.Header(seriesId, isSimplified).Value;
        
        header = header.WithIssuance(issuedAtUtc);

        Assert.Equal(header.IssuedAtUtc, issuedAtUtc);
    }
}