using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class InvoiceCreateDraftTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CreateDraft_WithValidData_SucceedsAndReturnsInvoiceDraft(bool isSimplified)
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
    public void CreateDraft_WithInvalidData_FailsAndNotReturnsInvoiceDraft()
    {
        const bool isSimplified = false;
        var id = Guid.Empty;
        var tenantId = Build.Tenant();
        
        var result = Invoice.CreateDraft(id, tenantId, isSimplified);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors, InvoiceError.IdInvalid);
    }
}

public class InvoiceChangeCustomerTests
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

public class InvoiceChangeCurrencyTests
{
    [Theory]
    [InlineData("EUR", "EUR")]
    [InlineData("eUr", "EUR")]
    [InlineData("   DOL       ", "DOL")]
    [InlineData(null, Invoice.DefaultCurrency)]
     
    public void ChangeCurrency_OfInvoiceDraftWithValidCurrencyCode_Succeeds(string? currencyCode, string expected)
    {
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false);

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
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false);

        var result = invoice.ChangeCurrency(currencyCode);

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors, InvoiceError.InvalidCurrencyCode);
    }
}

public class InvoiceChangeSeriesTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ChangeSeries_OfInvoiceDraftWithValidSeriesId_Succeeds(bool withSeries)
    {
        Guid? seriesId = withSeries ? Guid.CreateVersion7() : null;
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false);

        var result = invoice.ChangeSeries(seriesId);

        Assert.True(result.IsSuccess);
        Assert.Equal(seriesId, invoice.SeriesId);
    }

    [Fact]
    public void ChangeSeries_OfInvoiceDraftWithEmptyGuidSeriesId_Fails()
    {
        var seriesId = Guid.Empty;
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false);

        var result = invoice.ChangeSeries(seriesId);

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors, InvoiceError.SeriesInvalid);
    }
}

public class InvoiceChangeDetailsTests
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

public class InvoiceAddLineTests
{
    [Fact]
    public void AddLine_IntoInvoiceDraftWithASingleValidLine_Succeeds()
    {
        var invoiceId = Guid.CreateVersion7();
        var tenantId = Build.Tenant();
        var invoice = InvoiceBuilder.BuildDraft(id: invoiceId, tenantId: tenantId, isSimplified: false);
        
        var line = Build.CreateLine(tenantId, invoiceId).Value;

        var result = invoice.AddLine(line);

        Assert.True(result.IsSuccess);
        Assert.Equal(invoice.Id, line.InvoiceId);
        Assert.Equal(invoice.TenantId, line.TenantId);
        Assert.Single(invoice.Lines, line);
    }
    
    [Fact]
    public void AddLine_IntoInvoiceDraftWithSomeValidLines_Succeeds()
    {
        var invoiceId = Guid.CreateVersion7();
        var tenantId = Build.Tenant();
        var invoice = InvoiceBuilder.BuildDraft(id: invoiceId, tenantId: tenantId, isSimplified: false);
        
        var line1 = Build.CreateLine(tenantId, invoiceId, "Leek", units: 25, unitPrice: 1.5m, taxRate: 4).Value;
        var line2 = Build.CreateLine(tenantId, invoiceId, "Lettuce", units: 5, unitPrice: 5.5m, taxRate: 4).Value;
        var line3 = Build.CreateLine(tenantId, invoiceId, "Pumpkin", units: 14.54m, unitPrice: 1.4m, taxRate: 4).Value;
        var line4 = Build.CreateLine(tenantId, invoiceId, "Zucchini", units: 13.56m, unitPrice: 1.000m, taxRate: 4).Value;

        var result1 = invoice.AddLine(line1);
        var result2 = invoice.AddLine(line2);
        var result3 = invoice.AddLine(line3);

        IEnumerable<InvoiceLine> expectedLines = [line1, line2, line3];

        Assert.True(result1.IsSuccess && result2.IsSuccess &&  result3.IsSuccess);
        Assert.Equal(invoice.Id, line1.InvoiceId);
        Assert.Equal(invoice.Id, line2.InvoiceId);
        Assert.Equal(invoice.Id, line3.InvoiceId);
        Assert.Equal(invoice.TenantId, line1.TenantId);
        Assert.Equal(invoice.TenantId, line2.TenantId);
        Assert.Equal(invoice.TenantId, line3.TenantId);
        Assert.DoesNotContain(line4, invoice.Lines);
        Assert.Equivalent(expectedLines, invoice.Lines);
    }
    
    [Fact]
    public void AddLine_IntoInvoiceDraftWithSomeInvalidLines_FailsAtAddingInvalidLines()
    {
        var invoiceId = Guid.CreateVersion7();
        var tenantId = Build.Tenant();
        var invoice = InvoiceBuilder.BuildDraft(id: invoiceId, tenantId: tenantId, isSimplified: false);
        
        var lineDiffersInvoiceId = Build.CreateLine(tenantId, Guid.CreateVersion7(), "Leek", units: 25, unitPrice: 1.5m, taxRate: 4).Value;
        var lineDiffersTenantId = Build.CreateLine(Build.Tenant(), invoiceId, "Lettuce", units: 5, unitPrice: 5.5m, taxRate: 4).Value;
        var lineDiffersTenantAndInvoiceIds = Build.CreateLine(Build.Tenant(), Guid.CreateVersion7(), "Pumpkin", units: 14.54m, unitPrice: 1.4m, taxRate: 4).Value;
        var lineValid = Build.CreateLine(tenantId, invoiceId, "Tomato", units: 18.45m, unitPrice: 5.45m, taxRate: 4).Value;
        InvoiceLine? lineNull = null;
            
        var resultDiffersInvoiceId = invoice.AddLine(lineDiffersInvoiceId);
        var resultDiffersTenantId = invoice.AddLine(lineDiffersTenantId);
        var resultDiffersTenantAndInvoiceIds = invoice.AddLine(lineDiffersTenantAndInvoiceIds);
        var resultValid = invoice.AddLine(lineValid);
        var resultNull = invoice.AddLine(lineNull!);

        Assert.True(resultDiffersInvoiceId.IsFailure 
                    && resultDiffersTenantId.IsFailure 
                    && resultDiffersTenantAndInvoiceIds.IsFailure 
                    && resultValid.IsSuccess
                    && resultNull.IsFailure);
        Assert.Contains(InvoiceError.LineWithDifferentInvoiceIdentifier, resultDiffersInvoiceId.Errors);
        Assert.DoesNotContain(InvoiceError.LineWithDifferentTenantIdentifier, resultDiffersInvoiceId.Errors);
        Assert.DoesNotContain(InvoiceError.LineWithDifferentInvoiceIdentifier, resultDiffersTenantId.Errors);
        Assert.Contains(InvoiceError.LineWithDifferentTenantIdentifier, resultDiffersTenantId.Errors);
        Assert.Contains(InvoiceError.LineWithDifferentInvoiceIdentifier, resultDiffersTenantAndInvoiceIds.Errors);
        Assert.Contains(InvoiceError.LineWithDifferentTenantIdentifier, resultDiffersTenantAndInvoiceIds.Errors);
        Assert.Contains(InvoiceError.LineEmpty, resultNull.Errors);
        Assert.Empty(resultValid.Errors);
        Assert.Single(invoice.Lines, lineValid);
    }
}



public class InvoiceAddLinesTests
{
    [Fact]
    public void AddLines_WhenDraftAndLinesAreValid_ReturnsOkAndAddsAllLines()
    {
        var invoiceId = Guid.CreateVersion7();
        var tenantId = Build.Tenant();
        var invoice = InvoiceBuilder.BuildDraft(id: invoiceId, tenantId: tenantId, isSimplified: false);
        
        var lines  = Build.GetInvoiceLinesSampleList(tenantId, invoiceId);
        
        var result = invoice.AddLines(lines);
 
        Assert.True(result.IsSuccess);
        Assert.Equal(lines, invoice.Lines);
    }
    
    [Fact]
    public void AddLines_WhenInvoiceAlreadyHasLines_AppendsAndPreservesOrder()
    {
        var invoiceId = Guid.CreateVersion7();
        var tenantId = Build.Tenant();
        var existingLine = Build.GetValidInvoiceLine(tenantId, invoiceId);
        var invoice = InvoiceBuilder.BuildDraft(id: invoiceId, tenantId: tenantId, isSimplified: true)
                                    .WithLine(line: existingLine);
        
        var newLines  = Build.GetInvoiceLinesSampleList(tenantId, invoiceId);
            
        var result = invoice.AddLines(newLines);
 
        Assert.True(result.IsSuccess);
        Assert.Equal(invoice.Lines, new[] { existingLine }.Concat(newLines));
    }
    
    [Fact]
    public void AddLines_WhenLinesIsNull_FailsWithLinesCollectionEmpty()
    {
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false).WithLine();
        var existingLine = invoice.Lines[0];
 
        var result = invoice.AddLines(null!);
 
        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceError.LinesCollectionEmpty, result.Errors);
        Assert.Single(invoice.Lines, existingLine);
    }
    
    [Fact]
    public void AddLines_WhenLinesIsEmpty_FailsWithLinesCollectionEmpty()
    {
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false).WithLine();
        var existingLine = invoice.Lines[0];
    
        var result = invoice.AddLines([]);
    
        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceError.LinesCollectionEmpty, result.Errors);
        Assert.Single(invoice.Lines, existingLine);
    }
    
    [Fact]
    public void AddLines_WhenAnyLineIsNull_FailsAndIsAtomic_NoValidLineIsAdded()
    {
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: false).WithLine();
        var existingLine = invoice.Lines[0];

        var lines = Build.GetInvoiceLinesSampleList(invoice.TenantId, invoice.Id);
        lines.Add(null!);
    
        var result = invoice.AddLines(lines!);
    
        Assert.True(result.IsFailure);
        Assert.Contains(InvoiceError.LinesCollectionEmpty, result.Errors);
        Assert.Single(invoice.Lines, existingLine);
    }
}

public class InvoiceIssueTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 7, 12, 0, 0, TimeSpan.Zero);
    
    private static Invoice CreateDraftInvoice(
        bool isSimplified = false,
        bool hasCustomer = true,
        bool hasSeries = true,
        bool hasLines = true,
        bool hasDescription = true
        )
    {
        var invoice = InvoiceBuilder.BuildDraft(isSimplified: isSimplified);
        
        if(hasCustomer)
            invoice.WithCustomer(customerId: Guid.CreateVersion7());

        if(hasSeries)
            invoice.WithSeries(seriesId: Guid.CreateVersion7());
        
        if(hasDescription)
            invoice.WithDescription(description: "Sample sales description");
        
        if(hasLines)
            invoice.WithLine().WithLine().WithLine();
        
        return invoice;
    }
 
    [Fact]
    public void Issue_WhenNonSimplifiedInvoiceIsValid_ReturnsOkAndMarksAsIssued()
    {
        var invoice = CreateDraftInvoice();
 
        var result = invoice.Issue(Now.AddMinutes(-5), Now);
 
        Assert.True(result.IsSuccess);
        Assert.Equal(InvoiceStatus.Issued, invoice.Status);
        Assert.NotNull(invoice.IssuanceInfo);
    }
 
    [Fact]
    public void Issue_WhenSimplifiedInvoiceHasNoCustomer_ReturnsOkAndMarksAsIssued()
    {
        var invoice = CreateDraftInvoice(isSimplified: true, hasCustomer: false);
 
        var result = invoice.Issue(Now.AddMinutes(-5), Now);
 
        Assert.True(result.IsSuccess);
        Assert.Equal(InvoiceStatus.Issued, invoice.Status);
    }
 
    [Fact]
    public void Issue_WhenIssueDateEqualsNow_ReturnsOk()
    {
        var invoice = CreateDraftInvoice();
 
        var result = invoice.Issue(Now, Now);
 
        Assert.True(result.IsSuccess);
    }
 
    [Fact]
    public void Issue_WhenAlreadyIssued_FailsWithNotInDraftAndKeepsOriginalIssuanceInfo()
    {
        var invoice = CreateDraftInvoice();
        Assert.True(invoice.Issue(Now.AddMinutes(-5), Now).IsSuccess);
        var originalInfo = invoice.IssuanceInfo;
 
        var result = invoice.Issue(Now.AddMinutes(-1), Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(InvoiceError.NotInDraft, Assert.Single(result.Errors));
        Assert.Equal(InvoiceStatus.Issued, invoice.Status);
        Assert.Equal(originalInfo, invoice.IssuanceInfo);
    }
 
    [Fact]
    public void Issue_WhenNotDraftAndInputIsAlsoInvalid_ReturnsOnlyNotInDraft()
    {
        var invoice = CreateDraftInvoice();
        Assert.True(invoice.Issue(Now.AddMinutes(-5), Now).IsSuccess);
 
        var result = invoice.Issue(DateTimeOffset.MinValue, Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(InvoiceError.NotInDraft, Assert.Single(result.Errors));
    }
    
    [Fact]
    public void Issue_WhenNonSimplifiedInvoiceHasNoCustomer_FailsWithCustomerIdNeededAtNoSimplifiedInvoice()
    {
        var invoice = CreateDraftInvoice(isSimplified: false, hasCustomer: false);
 
        var result = invoice.Issue(Now.AddMinutes(-5), Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(InvoiceError.CustomerIdNeededAtNoSimplifiedInvoice, Assert.Single(result.Errors));
    }
    
    [Fact]
    public void Issue_WhenSeriesIsMissing_FailsWithSeriesRequired()
    {
        var invoice = CreateDraftInvoice(hasSeries: false);
 
        var result = invoice.Issue(Now.AddMinutes(-5), Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(InvoiceError.SeriesRequiredAtIssuanceTime, Assert.Single(result.Errors));
    }
    
    [Fact]
    public void Issue_WhenIssueDateIsMinValue_FailsWithIssueDateInvalidOnly()
    {
        var invoice = CreateDraftInvoice();
 
        var result = invoice.Issue(DateTimeOffset.MinValue, Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(IssuanceInfoError.IssueDateInvalid, Assert.Single(result.Errors));
        Assert.DoesNotContain(IssuanceInfoError.IssueDateIsFuture, result.Errors);
    }
 
    [Fact]
    public void Issue_WhenIssueDateIsInTheFuture_FailsWithIssueDateFuture()
    {
        var invoice = CreateDraftInvoice();
 
        var result = invoice.Issue(Now.AddDays(1), Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(IssuanceInfoError.IssueDateIsFuture, Assert.Single(result.Errors));
    }
 
    [Fact]
    public void Issue_WhenIssueDateIsOneTickAfterNow_FailsWithIssueDateFuture()
    {
        var invoice = CreateDraftInvoice();
 
        var result = invoice.Issue(Now.AddTicks(1), Now);
 
        Assert.Equal(IssuanceInfoError.IssueDateIsFuture, Assert.Single(result.Errors));
    }
 
    [Fact]
    public void Issue_WhenInvoiceHasNoLines_FailsWithInvoiceLinesEmpty()
    {
        var invoice = CreateDraftInvoice(hasLines: false);
 
        var result = invoice.Issue(Now.AddMinutes(-5), Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(InvoiceError.InvoiceLinesEmpty, Assert.Single(result.Errors));
    }
 
    [Fact]
    public void Issue_WhenSeveralRulesAreBroken_ReturnsAllErrorsAndDoesNotChangeState()
    {
        var invoice = CreateDraftInvoice(
            isSimplified: false, hasCustomer: false, hasSeries: false, hasLines: false);
 
        var result = invoice.Issue(DateTimeOffset.MinValue, Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(4, result.Errors.Count());
        Assert.Contains(InvoiceError.CustomerIdNeededAtNoSimplifiedInvoice, result.Errors);
        Assert.Contains(InvoiceError.SeriesRequiredAtIssuanceTime, result.Errors);
        Assert.Contains(IssuanceInfoError.IssueDateInvalid, result.Errors);
        Assert.Contains(InvoiceError.InvoiceLinesEmpty, result.Errors);
 
        Assert.Equal(InvoiceStatus.Draft, invoice.Status);
        Assert.Null(invoice.IssuanceInfo);
    }
}