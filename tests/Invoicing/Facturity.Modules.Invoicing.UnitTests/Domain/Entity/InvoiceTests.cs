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

// public class OtherTestsPendingToRefactor
// {
    //
//     [Fact]
//     public void CreateDraft_SimplifiedWithCustomer_Fails()
//     {
//         const string currencyCode = "EURRRRRRR";
//         var tenantId = Build.Tenant();
//         var series = Build.CreateSeries(tenantId).Value;
//         var result = Invoice.CreateDraft(tenantId, Guid.NewGuid(), 
//             Build.Header(series, true).Value, Build.Data(), currencyCode);
//
//         Error[] expectedErrors = [InvoiceError.CustomerIdNoAtSimplifiedInvoice, InvoiceError.InvalidCurrencyCode];
//         Assertions.FailsWith(result, expectedErrors);
//     }
//     
//     // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
//     //
//     // [Fact]
//     // public void CreateDraft_HeaderWithIssueDate_Fails()
//     // {
//     //     var header = Build.Header(Guid.NewGuid(), true).Value;
//     //
//     //     var result = Invoice.CreateDraft(Build.Tenant(), Guid.NewGuid(), header, Build.Data());
//     //
//     //     Assertions.FailsWith(result, InvoiceError.IssueDateAtInvoiceDraft);
//     // }
//     //
//     // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
//     // [Fact]
//     // public void CreateDraft_HeaderWithNumber_Fails()
//     // {
//     //     var header = Build.Header(Guid.NewGuid(), true).Value;
//     //
//     //     var result = Invoice.CreateDraft(Build.Tenant(), Guid.NewGuid(), header, Build.Data());
//     //
//     //     Assertions.FailsWith(result, InvoiceError.SeriesNumberSetAtDomain);
//     // }
//     //
//     // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
//     // [Fact]
//     // public void CreateDraft_MultipleViolations_ReturnsAllErrors()
//     // {
//     //     var header = Build.Header(Guid.NewGuid(), true).Value;
//     //
//     //     var result = Invoice.CreateDraft(Build.Tenant(), Guid.NewGuid(), header, Build.Data());
//     //
//     //     Assertions.FailsWith(result,
//     //         InvoiceError.CustomerIdAtSimplifiedInvoice,
//     //         InvoiceError.IssueDateAtInvoiceDraft,
//     //         InvoiceError.SeriesNumberSetAtDomain);
//     // }
//
//     [Fact]
//     public void CreateDraft_GeneratesDifferentIdsEachTime()
//     {
//         var a = Build.Draft();
//         var b = Build.Draft();
//
//         Assert.NotEqual(a.Id, b.Id);
//     }
// }
//
// public class UpdateDraftTests
// {
//     [Fact]
//     public void UpdateDraft_WithValidData_ReplacesState()
//     {
//         var invoice = Build.Draft();
//         var newCustomer = Guid.NewGuid();
//         var series = Build.CreateSeries().Value;
//         var newHeader = Build.Header(series, false).Value;
//         var newData = Build.Data(lines: 3);
//
//         var result = invoice.UpdateDraft(newCustomer, newHeader, newData);
//
//         Assert.True(result.IsSuccess);
//         Assert.Equal(newCustomer, invoice.CustomerId);
//         Assert.Same(newHeader, invoice.Header);
//         Assert.Same(newData, invoice.Data);
//         Assert.Equal(InvoiceStatus.Draft, invoice.Status);
//     }
//
//     [Fact]
//     public void UpdateDraft_ToSimplifiedWithoutCustomer_Succeeds()
//     {
//         var invoice = Build.Draft();
//         var series = Build.CreateSeries().Value;
//         var result = invoice.UpdateDraft(null, Build.Header(series, true).Value, Build.Data());
//
//         Assert.True(result.IsSuccess);
//         Assert.Null(invoice.CustomerId);
//         Assert.True(invoice.Header!.IsSimplified);
//     }
//
//     [Fact]
//     public void UpdateDraft_WhenNotDraft_FailsAndKeepsState()
//     {
//         var invoice = Build.Issued();
//         var customerBefore = invoice.CustomerId;
//         var headerBefore = invoice.Header;
//         var dataBefore = invoice.Data;
//         var series = Build.CreateSeries().Value;
//
//         var result = invoice.UpdateDraft(Guid.NewGuid(), Build.Header(series, true).Value, Build.Data(lines: 5));
//
//         Assertions.FailsWith(result, InvoiceError.NotInDraft);
//         Assert.Equal(customerBefore, invoice.CustomerId);
//         Assert.Same(headerBefore, invoice.Header);
//         Assert.Same(dataBefore, invoice.Data);
//         Assert.Equal(InvoiceStatus.Issued, invoice.Status);
//     }
//
//     [Fact]
//     public void UpdateDraft_WithInvalidData_FailsAndKeepsState()
//     {
//         const string currencyCode = "EURRRRRRR";
//         
//         var invoice = Build.Draft();
//         var customerBefore = invoice.CustomerId;
//         var headerBefore = invoice.Header;
//         var dataBefore = invoice.Data;
//         var series = Build.CreateSeries().Value;
//
//         var result = invoice.UpdateDraft(
//             customerId: Guid.NewGuid(), 
//             header: Build.Header(series, true).Value,
//             data: Build.Data(lines: 5),
//             currencyCode);
//
//         Assertions.FailsWith(result, [InvoiceError.CustomerIdNoAtSimplifiedInvoice, InvoiceError.InvalidCurrencyCode]);
//         Assert.Equal(customerBefore, invoice.CustomerId);
//         Assert.Same(headerBefore, invoice.Header);
//         Assert.Same(dataBefore, invoice.Data);
//     }
//
//     // [Fact]
//     // public void UpdateDraft_MultipleViolations_ReturnsAllErrors()
//     // {
//     //     var invoice = Build.Draft();
//     //     var header = Build.Header(Guid.NewGuid(), false).Value;
//     //
//     //     var result = invoice.UpdateDraft(Guid.NewGuid(), header, Build.Data());
//     //
//     //     Assertions.FailsWith(result,
//     //         InvoiceError.IssueDateAtInvoiceDraft,
//     //         InvoiceError.SeriesNumberSetAtDomain);
//     // }
// }
//
// public class IssueTests
// {
//     [Fact]
//     public void Issue_WithValidDraft_ChangesStatusAndSetsIssueDate()
//     {
//         var invoice = Build.Draft(simplified: false);
//         var issuedAt = Build.NowUtc.AddHours(-1);
//
//         var result = invoice.Issue(issuedAt, Build.NowUtc);
//
//         Assert.True(result.IsSuccess);
//         Assert.Equal(InvoiceStatus.Issued, invoice.Status);
//         Assert.Equal(issuedAt, invoice.Header!.IssuedAtUtc);
//         Assert.False(invoice.Header.IsSimplified);
//     }
//
//     [Fact]
//     public void Issue_SimplifiedWithoutCustomer_Succeeds()
//     {
//         var invoice = Build.Draft(simplified: true);
//
//         var result = invoice.Issue(Build.NowUtc, Build.NowUtc);
//
//         Assert.True(result.IsSuccess);
//         Assert.Equal(InvoiceStatus.Issued, invoice.Status);
//     }
//
//     [Fact]
//     public void Issue_WhenIssueDateEqualsNow_Succeeds()
//     {
//         var invoice = Build.Draft();
//
//         var result = invoice.Issue(Build.NowUtc, Build.NowUtc);
//
//         Assert.True(result.IsSuccess);
//     }
//
//     [Fact]
//     public void Issue_WhenIssueDateIsInTheFuture_FailsAndStaysDraft()
//     {
//         var invoice = Build.Draft();
//         var headerBefore = invoice.Header;
//
//         var result = invoice.Issue(Build.NowUtc.AddTicks(1), Build.NowUtc);
//
//         Assertions.FailsWith(result, InvoiceError.IssueDateFuture);
//         Assert.Equal(InvoiceStatus.Draft, invoice.Status);
//         Assert.Same(headerBefore, invoice.Header);
//         Assert.Null(invoice.Header!.IssuedAtUtc);
//     }
//
//     [Fact]
//     public void Issue_WhenAlreadyIssued_FailsWithNotInDraft()
//     {
//         var invoice = Build.Issued();
//         var headerBefore = invoice.Header;
//
//         var result = invoice.Issue(Build.NowUtc, Build.NowUtc);
//
//         Assertions.FailsWith(result, InvoiceError.NotInDraft);
//         Assert.Equal(InvoiceStatus.Issued, invoice.Status);
//         Assert.Same(headerBefore, invoice.Header);
//     }
//
//     [Fact]
//     public void Issue_WhenNotDraft_ReportsOnlyNotInDraft_EvenWithFutureDate()
//     {
//         var invoice = Build.Issued();
//
//         var result = invoice.Issue(Build.NowUtc.AddDays(1), Build.NowUtc);
//
//         Assertions.FailsWith(result, InvoiceError.NotInDraft);
//     }
//
//     [Fact]
//     public void Issue_WithoutHeader_Fails()
//     {
//         var invoice = Build.DraftWith(Guid.NewGuid(), null, Build.Data());
//
//         var result = invoice.Issue(Build.NowUtc, Build.NowUtc);
//
//         Assertions.FailsWith(result, InvoiceError.NullInvoiceHeader);
//         Assert.Equal(InvoiceStatus.Draft, invoice.Status);
//     }
//
//     [Fact]
//     public void Issue_WithoutData_Fails()
//     {
//         var series = Build.CreateSeries().Value;
//         var invoice = Build.DraftWith(Guid.NewGuid(), Build.Header(series, false).Value, null);
//
//         var result = invoice.Issue(Build.NowUtc, Build.NowUtc);
//
//         Assertions.FailsWith(result, InvoiceError.NullInvoiceData);
//         Assert.Equal(InvoiceStatus.Draft, invoice.Status);
//     }
//
//     // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
//     // [Fact]
//     // public void Issue_WithNoLines_Fails()
//     // {
//     //     var invoice = Build.DraftWith(null, Build.Header(Guid.NewGuid(), true).Value, Build.Data(lines: 0));
//     //
//     //     var result = invoice.Issue(Build.Now, Build.Now);
//     //
//     //     Assertions.FailsWith(result, InvoiceError.NoInvoiceLines);
//     //     Assert.Equal(InvoiceStatus.Draft, invoice.Status);
//     // }
//
//     // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
//     // [Fact]
//     // public void Issue_NonSimplifiedWithoutCustomer_Fails()
//     // {
//     //     var invoice = Build.DraftWith(Guid.NewGuid(), Build.Header(Guid.NewGuid(), false).Value, Build.Data());
//     //
//     //     var result = invoice.Issue(Build.Now, Build.Now);
//     //
//     //     Assertions.FailsWith(result, InvoiceError.MissingCustomerIdAtNoSimplifiedInvoice);
//     //     Assert.Equal(InvoiceStatus.Draft, invoice.Status);
//     // }
//
//     [Fact]
//     public void Issue_AccumulatesAllErrors()
//     {
//         var invoice = Build.DraftWith(null, null, null);
//
//         var result = invoice.Issue(Build.NowUtc.AddDays(1), Build.NowUtc);
//
//         Assertions.FailsWith(result,
//             InvoiceError.NullInvoiceHeader,
//             InvoiceError.NullInvoiceData,
//             InvoiceError.IssueDateFuture);
//     }
//
//     // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
//     // [Fact]
//     // public void Issue_AccumulatesCustomerAndLinesErrors()
//     // {
//     //     var invoice = Build.DraftWith(null, Build.Header(Guid.NewGuid(), true).Value, Build.Data(lines: 0));
//     //
//     //     var result = invoice.Issue(Build.Now, Build.Now);
//     //
//     //     Assertions.FailsWith(result,
//     //         InvoiceError.MissingCustomerIdAtNoSimplifiedInvoice,
//     //         InvoiceError.NoInvoiceLines);
//     // }
//
//     [Fact]
//     public void UpdateDraft_AfterIssue_Fails()
//     {
//         var invoice = Build.Issued();
//         var series = Build.CreateSeries().Value;
//
//         var result = invoice.UpdateDraft(Guid.NewGuid(), Build.Header(series, true).Value, Build.Data());
//
//         Assertions.FailsWith(result, InvoiceError.NotInDraft);
//     }
// }
//
// public class TaxBreakdownTests
// {
//     [Fact]
//     public void TaxBreakdown_WithoutData_IsEmpty()
//     {
//         var series = Build.CreateSeries().Value;
//         var invoice = Build.DraftWith(Guid.NewGuid(), Build.Header(series, false).Value, null);
//
//         Assert.Empty(invoice.TaxBreakdown);
//     }
//
//     [Fact]
//     public void TaxBreakdown_WithLines_MatchesBreakdownLineCreation()
//     {
//         var data = Build.Data(lines: 3);
//         var series = Build.CreateSeries().Value;
//         var invoice = Build.DraftWith(Guid.NewGuid(), Build.Header(series, false).Value, data);
//
//         var expected = BreakdownLine.CreateList(data.Lines);
//
//         Assert.NotEmpty(invoice.TaxBreakdown);
//         Assert.Equal(expected, invoice.TaxBreakdown);
//     }
//
//     [Fact]
//     public void TaxBreakdown_ReflectsUpdatedData()
//     {
//         var invoice = Build.Draft();
//         var newData = Build.Data(lines: 4);
//
//         invoice.UpdateDraft(invoice.CustomerId, invoice.Header, newData);
//
//         Assert.Equal(BreakdownLine.CreateList(newData.Lines), invoice.TaxBreakdown);
//     }
// }

internal static class Assertions
{
    public static void FailsWith(Result result, params Error[] expected)
    {
        Assert.False(result.IsSuccess);
        var actual = result.Errors.ToList();
        Assert.Equal(expected.Length, actual.Count);
        foreach (var error in expected)
            Assert.Contains(error, actual);
    }
    
    public static void FailsWith<T>(Result<T> result, params Error[] expected)
    {
        Assert.False(result.IsSuccess);
        var actual = result.Errors.ToList();
        Assert.Equal(expected.Length, actual.Count);
        foreach (var error in expected)
            Assert.Contains(error, actual);
    }
}