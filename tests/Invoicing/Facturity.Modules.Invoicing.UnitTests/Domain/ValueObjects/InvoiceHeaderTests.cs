using Facturity.Modules.Invoicing.UnitTests.Domain;
using Microsoft.Extensions.Time.Testing;

namespace Facturity.Modules.Invoicing.Domain;

public class InvoiceHeaderTests
{
    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Create_WithValidData_ShouldSuccessReturningInvoiceHeader(bool withIssuedAtUtc, bool isSimplified)
    {
        var seriesId = Guid.NewGuid(); 
        
        var fakeCurrentTime = new DateTimeOffset(2026, 12, 30, 08, 00,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        DateTimeOffset? issuedAtUtc = withIssuedAtUtc
            ? new DateTimeOffset(2026, 12, 28, 08, 00, 00, TimeSpan.Zero)
            : null;
        
        var result = Helper.CreateInvoiceHeader(seriesId, isSimplified, issuedAtUtc, fakeTimeProvider);

        Assert.True(result.IsSuccess);
        Assert.Equal(seriesId, result.Value.SeriesId);
        Assert.Equal(isSimplified, result.Value.IsSimplified);
        Assert.Equal(issuedAtUtc, result.Value.IssuedAtUtc);
    }

    [Fact]
    public void Create_WithFutureIssuedAtUtc_ShouldFail()
    {
        var seriesId = Guid.NewGuid();
        const bool isSimplified = false;
        
        var fakeCurrentTime = new DateTimeOffset(2026, 11, 29, 09, 00,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);

        var issuedAtUtc = new DateTimeOffset(2026, 11, 30, 08, 00, 00, TimeSpan.Zero);
        
        var result = Helper.CreateInvoiceHeader(seriesId, isSimplified, issuedAtUtc, fakeTimeProvider);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceHeaderError.FutureDateIssuedAtUtc, result.Errors);
        Assert.DoesNotContain(InvoiceHeaderError.SeriesIdGuiEmpty, result.Errors);
    }

    [Fact]
    public void Create_WithoutSeriesId_ShouldFail()
    {
        var seriesId = Guid.Empty;
        const bool isSimplified = false;
        
        var fakeCurrentTime = new DateTimeOffset(2026, 12, 29, 09, 00,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);

        DateTimeOffset? issuedAtUtc = null;
        
        var result = Helper.CreateInvoiceHeader(seriesId, isSimplified, issuedAtUtc, fakeTimeProvider);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceHeaderError.SeriesIdGuiEmpty, result.Errors);
        Assert.DoesNotContain(InvoiceHeaderError.FutureDateIssuedAtUtc, result.Errors);
    }

}