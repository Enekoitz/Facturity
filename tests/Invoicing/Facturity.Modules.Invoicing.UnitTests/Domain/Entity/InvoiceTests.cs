using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

// CreateDraft
// ChangeCustomer (Must be from Draft)
// ChangeSeries (Must be from Draft)
// ChangeDetails (Must be from Draft)
// ChangeCurrency (Must be from Draft)
// AddLine (Must be from Draft)
// AddLines (Must be from Draft)
// Issue (Must be from Draft)

public class CreateDraftTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CreateDraft_WithValidData_SucceedsReturningInvoiceDraft(bool isSimplified)
    {
        var id = Guid.CreateVersion7();
        var tenantId = Build.Tenant();

        var result = Invoice.CreateDraft(id, tenantId, isSimplified);
        
        Assert.True(result.IsSuccess);
        
        var invoice = result.Value;
        
        Assert.NotEqual(Guid.Empty, invoice.Id);
        Assert.Equal(id, invoice.Id);
        Assert.Equal(tenantId, invoice.TenantId);
        Assert.Equal(isSimplified, invoice.IsSimplified);
        Assert.Null(invoice.TransactionAtUtc);
        Assert.Null(invoice.CustomerId);
        Assert.Null(invoice.Series);
        Assert.Null(invoice.SeriesId);
        Assert.Null(invoice.IssuanceInfo);
        Assert.Null(invoice.Description);
        Assert.Empty(invoice.Lines);
        Assert.Equal(0, invoice.TotalAmount().Amount);
        Assert.Empty(invoice.TaxBreakdown);
        Assert.Equal(Invoice.DefaultCurrency, invoice.Currency);
        Assert.Equal(InvoiceStatus.Draft, invoice.Status);
    }
    
    [Fact]
    public void CreateDraft_WithInvalidData_FailsNotReturningInvoiceDraft()
    {
        const bool isSimplified = false;
        var id = Guid.Empty;
        var tenantId = Build.Tenant();
        
        var result = Invoice.CreateDraft(id, tenantId, isSimplified);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors, InvoiceError.IdInvalid);
    }
}

public class ChangeCustomerTests
{
    [Fact]
    public void ChangeCustomer_OfNonSimplifiedInvoiceDraftWithNonEmptyCustomerId_Succeeds()
    {
        const bool isSimplified = false;
        var customerId = Guid.CreateVersion7();
        var invoice = InvoiceBuilder.BuildDraft(isSimplified);
         
        var result = invoice.ChangeCustomer(customerId);
     
        Assert.True(result.IsSuccess);
        Assert.Equal(customerId, invoice.CustomerId);
    }
     
    [Fact]
    public void ChangeCustomer_OfNonSimplifiedInvoiceDraftWithNullCustomerId_Succeeds()
    {
        const bool isSimplified = false;
        Guid? customerId = null;
        var invoice = InvoiceBuilder.BuildDraft(isSimplified);
         
        var result = invoice.ChangeCustomer(customerId);
     
        Assert.True(result.IsSuccess);
        Assert.Null(invoice.CustomerId);
    }
     
    [Fact]
    public void ChangeCustomer_OfSimplifiedInvoiceDraftWithNonEmptyCustomerId_Fails()
    {
        const bool isSimplified = true;
        var customerId = Guid.CreateVersion7();
        var invoice = InvoiceBuilder.BuildDraft(isSimplified);

        var result = invoice.ChangeCustomer(customerId);

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors, InvoiceError.CustomerIdNoAtSimplifiedInvoice);
    }
     
    // PDTE. ChangeCustomer_OfNonSimplifiedIssuedInvoiceWithNonEmptyCustomerId_Fails 
    // once we have developed builder methods of IssueInvoice
}

public class ChangeCurrencyTests
{
    [Theory]
    [InlineData("EUR", "EUR")]
    [InlineData("eUr", "EUR")]
    [InlineData("   DOL       ", "DOL")]
    [InlineData(null, Invoice.DefaultCurrency)]
     
    public void ChangeCurrency_OfInvoiceDraftWithValidCurrencyCode_Succeeds(string? currencyCode, string expected)
    {
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false)
            .WithCustomer(customerId: Guid.CreateVersion7());

        var result = invoice.ChangeCurrency(currencyCode);

        Assert.True(result.IsSuccess);
        Assert.Equal(expected, invoice.Currency);
    }

    [Theory]
    [InlineData("EURRRR")]
    [InlineData("eu")]
    [InlineData("")]
     
    public void ChangeCurrency_OfInvoiceDraftWithInvalidCurrencyCode_Fails(string? currencyCode)
    {
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false)
            .WithCustomer(customerId: Guid.CreateVersion7());

        var result = invoice.ChangeCurrency(currencyCode);

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors, InvoiceError.InvalidCurrencyCode);
    }
}

public class ChangeSeriesTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ChangeSeries_OfInvoiceDraftWithValidSeriesId_Succeeds(bool withSeries)
    {
        Guid? seriesId = withSeries ? Guid.CreateVersion7() : null;
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false)
            .WithCustomer(customerId: Guid.CreateVersion7());

        var result = invoice.ChangeSeries(seriesId);

        Assert.True(result.IsSuccess);
        Assert.Equal(seriesId, invoice.SeriesId);
    }

    [Fact]
    public void ChangeSeries_OfInvoiceDraftWithEmptyGuidSeriesId_Fails()
    {
        var seriesId = Guid.Empty;
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false)
            .WithCustomer(customerId: Guid.CreateVersion7());

        var result = invoice.ChangeSeries(seriesId);

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors, InvoiceError.SeriesInvalid);
    }
}

public class ChangeDetailsTests
{
    [Theory]
    [InlineData("Venta de hortalizas.   ",false)]
    [InlineData("   description ", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    [InlineData("Exactly description maximus length (250). Exactly description maximus length. Exactly description maximus length. Exactly description maximus length. Exactly description maximus length. Exactly description maximus length. Exactly description maximus.", true)]
    public void ChangeDetails_OfInvoiceDraftWithValidData_Succeeds(string? description, bool withTransactionDate)
    {
        var nowUtc = new DateTimeOffset(2026, 11, 07, 08,00,00, TimeSpan.Zero).ToUniversalTime();
        DateTimeOffset? transactionAtUtc = withTransactionDate ? nowUtc.AddDays(-1) : null;
        description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false);

        var result = invoice.ChangeDetails(description, transactionAtUtc, nowUtc);

        Assert.True(result.IsSuccess);
        Assert.Equal(description, invoice.Description);
        Assert.Equal(transactionAtUtc, invoice.TransactionAtUtc);
    }

    [Fact]
    public void ChangeDetails_OfInvoiceDraftWithDescriptionTooLong_Fails()
    {
        DateTimeOffset? transactionAtUtc = null;
        var nowUtc = new DateTimeOffset(2026, 11, 07, 08,00,00, TimeSpan.Zero).ToUniversalTime();
        var description = Utilities.GetRandomLength(Invoice.DescriptionMaxLength + 1);
        
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false);

        var result = invoice.ChangeDetails(description, transactionAtUtc, nowUtc);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceError.DescriptionTooLong, result.Errors);
    }
    
    [Fact]
    public void ChangeDetails_OfInvoiceDraftWithTransactionDateFuture_Fails()
    {
        var nowUtc = new DateTimeOffset(2026, 11, 07, 
                                                    08,43,59, 
                                                    TimeSpan.Zero).ToUniversalTime(); // 2026-11-07 08:43:59
        DateTimeOffset? transactionAtUtc = nowUtc.AddDays(4);  // 2026-11-11 08:43:59
        
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false);

        var result = invoice.ChangeDetails(description: null, transactionAtUtc, nowUtc);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceError.TransactionDateIsFuture, result.Errors);
    }
    
    [Fact]
    public void ChangeDetails_OfInvoiceDraftWithTransactionDateMinValue_Fails()
    {
        Assert.False(result.IsSuccess);
        var actual = result.Errors.ToList();
        Assert.Equal(expected.Length, actual.Count);
        foreach (var error in expected)
            Assert.Contains(error, actual);
    }
    
    public static void FailsWith<T>(Result<T> result, params Error[] expected)
        var nowUtc = new DateTimeOffset(2026, 11, 07, 
            08,43,59, 
            TimeSpan.Zero).ToUniversalTime(); // 2026-11-07 08:43:59
        DateTimeOffset? transactionAtUtc = DateTimeOffset.MinValue;  // 1/1/0001 0:00:00 +00:00
        
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false);

        var result = invoice.ChangeDetails(description: null, transactionAtUtc, nowUtc);

        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceError.TransactionDateInvalid, result.Errors);
    }
}
    {
        Assert.False(result.IsSuccess);
        var actual = result.Errors.ToList();
        Assert.Equal(expected.Length, actual.Count);
        foreach (var error in expected)
            Assert.Contains(error, actual);
    }
}