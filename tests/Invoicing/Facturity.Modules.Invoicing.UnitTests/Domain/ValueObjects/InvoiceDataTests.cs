using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.Extensions.Time.Testing;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class InvoiceDataTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_WithValidData_ShouldReturnSuccessInvoiceDataResult(bool nullTransactionAtUtc)
    {
        const string description = $"Invoice description example. Less than 500 characters length.      ";
        var invoiceLines = Build.GetInvoiceLinesSampleList();

        var fakeCurrentTime = new DateTimeOffset(2026, 12, 30, 08, 00,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        DateTimeOffset? transactionAtUtc = nullTransactionAtUtc
            ? new DateTimeOffset(2026, 12, 28, 08, 00, 00, TimeSpan.Zero)
            : null;
       
        var result = InvoiceData.Create(description, invoiceLines, transactionAtUtc, fakeCurrentTime);
        
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrBlankDescription_ShouldFailWithoutThrowing(string? description)
    {
        var invoiceLines = Build.GetInvoiceLinesSampleList();

        var fakeCurrentTime = new DateTimeOffset(2026, 11, 30, 08, 00,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);

        DateTimeOffset? transactionAtUtc = 
            new DateTimeOffset(2026, 11, 28, 07, 55, 00, TimeSpan.Zero);
       
        var result = InvoiceData.Create(description, invoiceLines, transactionAtUtc, fakeCurrentTime);
        
        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceDataError.NullOrEmptyDescription, result.Errors);
    }

    [Fact]
    public void Create_WithDescriptionOverMaxLength_ShouldFail()
    {
        var description = Utilities.GetRandomLength(InvoiceData.DescriptionMaxLength + 1);
        var invoiceLines = Build.GetInvoiceLinesSampleList();

        var fakeCurrentTime = new DateTimeOffset(2026, 11, 30, 08, 00,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);

        DateTimeOffset? transactionAtUtc = 
            new DateTimeOffset(2026, 11, 28, 07, 55, 00, TimeSpan.Zero);
       
        var result = InvoiceData.Create(description, invoiceLines, transactionAtUtc, fakeCurrentTime);
        
        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceDataError.ExceededMaximumLengthDescription, result.Errors);
    }

    [Fact]
    public void Create_ShouldTrimDescription()
    {
        const string description = "  Invoice description example to trim  ";
        var invoiceLines = Build.GetInvoiceLinesSampleList();

        var fakeCurrentTime = new DateTimeOffset(2026, 11, 30, 08, 00,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);

        DateTimeOffset? transactionAtUtc = null;
       
        var result = InvoiceData.Create(description, invoiceLines, transactionAtUtc, fakeCurrentTime);

        Assert.Equal("Invoice description example to trim", result.Value.Description);
    }
    
    [Fact]
    public void Create_WithFutureTransactionAtUtc_ShouldFail()
    {
        const string description = "Invoice description example";
        var invoiceLines = Build.GetInvoiceLinesSampleList();

        var fakeCurrentTime = new DateTimeOffset(2026, 05, 14, 16, 32,43, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);

        DateTimeOffset? transactionAtUtc = 
            new DateTimeOffset(2027, 01, 30, 05, 23, 14, TimeSpan.Zero);
       
        var result = InvoiceData.Create(description, invoiceLines, transactionAtUtc, fakeCurrentTime);
        
        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceDataError.FutureDateTransactionAtUtc, result.Errors);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_WithEmptyInvoiceLinesList_ShouldFail(bool nullList)
    {
        const string description = "Invoice test";

        var fakeCurrentTime = new DateTimeOffset(2026, 05, 14, 16, 32,43, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);

        DateTimeOffset? transactionAtUtc = 
            new DateTimeOffset(2026, 01, 30, 05, 23, 14, TimeSpan.Zero);
       
        var invoiceLines = nullList 
            ? null 
            : new List<InvoiceLine>() ;
        
        var result = InvoiceData.Create(description, invoiceLines, transactionAtUtc, fakeCurrentTime);
        
        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceDataError.EmptyInvoiceLinesList, result.Errors);
    }
}