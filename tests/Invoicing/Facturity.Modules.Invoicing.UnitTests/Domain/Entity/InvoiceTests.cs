using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class CreateDraftTests
{
    [Fact]
    public void CreateDraft_WithValidData_ReturnsDraftInvoice()
    {
        var tenant = Build.Tenant();
        var customerId = Guid.NewGuid();
        var header = Build.Header(Guid.NewGuid(), false).Value;
        var data = Build.Data(lines: 2);

        var result = Invoice.CreateDraft(tenant, customerId, header, data);

        Assert.True(result.IsSuccess);
        var invoice = result.Value;
        Assert.NotEqual(Guid.Empty, invoice.Id);
        Assert.Equal(tenant, invoice.TenantId);
        Assert.Equal(customerId, invoice.CustomerId);
        Assert.Same(header, invoice.Header);
        Assert.Same(data, invoice.Data);
        Assert.Equal(InvoiceStatus.Draft, invoice.Status);
    }

    [Fact]
    public void CreateDraft_WithoutHeaderAndData_Succeeds()
    {
        var result = Invoice.CreateDraft(Build.Tenant(), null, null, null);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Value.Header);
        Assert.Null(result.Value.Data);
        Assert.Equal(InvoiceStatus.Draft, result.Value.Status);
    }

    [Theory]
    [InlineData(false, true)] 
    [InlineData(false, false)]  
    [InlineData(true, false)]
    public void CreateDraft_AllowedCustomerCombinations_Succeed(bool simplified, bool withCustomer)
    {
        Guid? customerId = withCustomer ? Guid.NewGuid() : null;

        var result = Invoice.CreateDraft(Build.Tenant(), customerId, Build.Header(Guid.NewGuid(), simplified).Value, Build.Data());

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CreateDraft_SimplifiedWithCustomer_Fails()
    {
        var result = Invoice.CreateDraft(Build.Tenant(), Guid.NewGuid(), Build.Header(Guid.NewGuid(), true).Value, Build.Data());

        Assertions.FailsWith(result, InvoiceError.CustomerIdAtSimplifiedInvoice);
    }
    
    // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
    //
    // [Fact]
    // public void CreateDraft_HeaderWithIssueDate_Fails()
    // {
    //     var header = Build.Header(Guid.NewGuid(), true).Value;
    //
    //     var result = Invoice.CreateDraft(Build.Tenant(), Guid.NewGuid(), header, Build.Data());
    //
    //     Assertions.FailsWith(result, InvoiceError.IssueDateAtInvoiceDraft);
    // }
    //
    // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
    // [Fact]
    // public void CreateDraft_HeaderWithNumber_Fails()
    // {
    //     var header = Build.Header(Guid.NewGuid(), true).Value;
    //
    //     var result = Invoice.CreateDraft(Build.Tenant(), Guid.NewGuid(), header, Build.Data());
    //
    //     Assertions.FailsWith(result, InvoiceError.SeriesNumberSetAtDomain);
    // }
    //
    // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
    // [Fact]
    // public void CreateDraft_MultipleViolations_ReturnsAllErrors()
    // {
    //     var header = Build.Header(Guid.NewGuid(), true).Value;
    //
    //     var result = Invoice.CreateDraft(Build.Tenant(), Guid.NewGuid(), header, Build.Data());
    //
    //     Assertions.FailsWith(result,
    //         InvoiceError.CustomerIdAtSimplifiedInvoice,
    //         InvoiceError.IssueDateAtInvoiceDraft,
    //         InvoiceError.SeriesNumberSetAtDomain);
    // }

    [Fact]
    public void CreateDraft_GeneratesDifferentIdsEachTime()
    {
        var a = Build.Draft();
        var b = Build.Draft();

        Assert.NotEqual(a.Id, b.Id);
    }
}

public class UpdateDraftTests
{
    [Fact]
    public void UpdateDraft_WithValidData_ReplacesState()
    {
        var invoice = Build.Draft();
        var newCustomer = Guid.NewGuid();
        var newHeader =Build.Header(Guid.NewGuid(), false).Value;
        var newData = Build.Data(lines: 3);

        var result = invoice.UpdateDraft(newCustomer, newHeader, newData);

        Assert.True(result.IsSuccess);
        Assert.Equal(newCustomer, invoice.CustomerId);
        Assert.Same(newHeader, invoice.Header);
        Assert.Same(newData, invoice.Data);
        Assert.Equal(InvoiceStatus.Draft, invoice.Status);
    }

    [Fact]
    public void UpdateDraft_ToSimplifiedWithoutCustomer_Succeeds()
    {
        var invoice = Build.Draft();

        var result = invoice.UpdateDraft(null, Build.Header(Guid.NewGuid(), true).Value, Build.Data());

        Assert.True(result.IsSuccess);
        Assert.Null(invoice.CustomerId);
        Assert.True(invoice.Header!.IsSimplified);
    }

    [Fact]
    public void UpdateDraft_WhenNotDraft_FailsAndKeepsState()
    {
        var invoice = Build.Issued();
        var customerBefore = invoice.CustomerId;
        var headerBefore = invoice.Header;
        var dataBefore = invoice.Data;

        var result = invoice.UpdateDraft(Guid.NewGuid(), Build.Header(Guid.NewGuid(), true).Value, Build.Data(lines: 5));

        Assertions.FailsWith(result, InvoiceError.NotInDraft);
        Assert.Equal(customerBefore, invoice.CustomerId);
        Assert.Same(headerBefore, invoice.Header);
        Assert.Same(dataBefore, invoice.Data);
        Assert.Equal(InvoiceStatus.Issued, invoice.Status);
    }

    [Fact]
    public void UpdateDraft_WithInvalidData_FailsAndKeepsState()
    {
        var invoice = Build.Draft();
        var customerBefore = invoice.CustomerId;
        var headerBefore = invoice.Header;
        var dataBefore = invoice.Data;

        var result = invoice.UpdateDraft(
            customerId: Guid.NewGuid(), 
            header: Build.Header(Guid.NewGuid(), true).Value,
            data: Build.Data(lines: 5));

        Assertions.FailsWith(result, InvoiceError.CustomerIdAtSimplifiedInvoice);
        Assert.Equal(customerBefore, invoice.CustomerId);
        Assert.Same(headerBefore, invoice.Header);
        Assert.Same(dataBefore, invoice.Data);
    }

    // [Fact]
    // public void UpdateDraft_MultipleViolations_ReturnsAllErrors()
    // {
    //     var invoice = Build.Draft();
    //     var header = Build.Header(Guid.NewGuid(), false).Value;
    //
    //     var result = invoice.UpdateDraft(Guid.NewGuid(), header, Build.Data());
    //
    //     Assertions.FailsWith(result,
    //         InvoiceError.IssueDateAtInvoiceDraft,
    //         InvoiceError.SeriesNumberSetAtDomain);
    // }
}

public class IssueTests
{
    [Fact]
    public void Issue_WithValidDraft_ChangesStatusAndSetsIssueDate()
    {
        var invoice = Build.Draft(simplified: false);
        var issuedAt = Build.Now.AddHours(-1);

        var result = invoice.Issue(issuedAt, Build.Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(InvoiceStatus.Issued, invoice.Status);
        Assert.Equal(issuedAt, invoice.Header!.IssuedAtUtc);
        Assert.False(invoice.Header.IsSimplified);
    }

    [Fact]
    public void Issue_SimplifiedWithoutCustomer_Succeeds()
    {
        var invoice = Build.Draft(simplified: true);

        var result = invoice.Issue(Build.Now, Build.Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(InvoiceStatus.Issued, invoice.Status);
    }

    [Fact]
    public void Issue_WhenIssueDateEqualsNow_Succeeds()
    {
        var invoice = Build.Draft();

        var result = invoice.Issue(Build.Now, Build.Now);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Issue_WhenIssueDateIsInTheFuture_FailsAndStaysDraft()
    {
        var invoice = Build.Draft();
        var headerBefore = invoice.Header;

        var result = invoice.Issue(Build.Now.AddTicks(1), Build.Now);

        Assertions.FailsWith(result, InvoiceError.FutureIssueDate);
        Assert.Equal(InvoiceStatus.Draft, invoice.Status);
        Assert.Same(headerBefore, invoice.Header);
        Assert.Null(invoice.Header!.IssuedAtUtc);
    }

    [Fact]
    public void Issue_WhenAlreadyIssued_FailsWithNotInDraft()
    {
        var invoice = Build.Issued();
        var headerBefore = invoice.Header;

        var result = invoice.Issue(Build.Now, Build.Now);

        Assertions.FailsWith(result, InvoiceError.NotInDraft);
        Assert.Equal(InvoiceStatus.Issued, invoice.Status);
        Assert.Same(headerBefore, invoice.Header);
    }

    [Fact]
    public void Issue_WhenNotDraft_ReportsOnlyNotInDraft_EvenWithFutureDate()
    {
        var invoice = Build.Issued();

        var result = invoice.Issue(Build.Now.AddDays(1), Build.Now);

        Assertions.FailsWith(result, InvoiceError.NotInDraft);
    }

    [Fact]
    public void Issue_WithoutHeader_Fails()
    {
        var invoice = Build.DraftWith(Guid.NewGuid(), null, Build.Data());

        var result = invoice.Issue(Build.Now, Build.Now);

        Assertions.FailsWith(result, InvoiceError.NullInvoiceHeader);
        Assert.Equal(InvoiceStatus.Draft, invoice.Status);
    }

    [Fact]
    public void Issue_WithoutData_Fails()
    {
        var invoice = Build.DraftWith(Guid.NewGuid(), Build.Header(Guid.NewGuid(), false).Value, null);

        var result = invoice.Issue(Build.Now, Build.Now);

        Assertions.FailsWith(result, InvoiceError.NullInvoiceData);
        Assert.Equal(InvoiceStatus.Draft, invoice.Status);
    }

    // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
    // [Fact]
    // public void Issue_WithNoLines_Fails()
    // {
    //     var invoice = Build.DraftWith(null, Build.Header(Guid.NewGuid(), true).Value, Build.Data(lines: 0));
    //
    //     var result = invoice.Issue(Build.Now, Build.Now);
    //
    //     Assertions.FailsWith(result, InvoiceError.NoInvoiceLines);
    //     Assert.Equal(InvoiceStatus.Draft, invoice.Status);
    // }

    // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
    // [Fact]
    // public void Issue_NonSimplifiedWithoutCustomer_Fails()
    // {
    //     var invoice = Build.DraftWith(Guid.NewGuid(), Build.Header(Guid.NewGuid(), false).Value, Build.Data());
    //
    //     var result = invoice.Issue(Build.Now, Build.Now);
    //
    //     Assertions.FailsWith(result, InvoiceError.MissingCustomerIdAtNoSimplifiedInvoice);
    //     Assert.Equal(InvoiceStatus.Draft, invoice.Status);
    // }

    [Fact]
    public void Issue_AccumulatesAllErrors()
    {
        var invoice = Build.DraftWith(null, null, null);

        var result = invoice.Issue(Build.Now.AddDays(1), Build.Now);

        Assertions.FailsWith(result,
            InvoiceError.NullInvoiceHeader,
            InvoiceError.NullInvoiceData,
            InvoiceError.FutureIssueDate);
    }

    // TODO: ARREGLAR EL SIGUIENTE TEST TRAS REFACTORIZAR TODO EL DOMAIN 
    // [Fact]
    // public void Issue_AccumulatesCustomerAndLinesErrors()
    // {
    //     var invoice = Build.DraftWith(null, Build.Header(Guid.NewGuid(), true).Value, Build.Data(lines: 0));
    //
    //     var result = invoice.Issue(Build.Now, Build.Now);
    //
    //     Assertions.FailsWith(result,
    //         InvoiceError.MissingCustomerIdAtNoSimplifiedInvoice,
    //         InvoiceError.NoInvoiceLines);
    // }

    [Fact]
    public void UpdateDraft_AfterIssue_Fails()
    {
        var invoice = Build.Issued();

        var result = invoice.UpdateDraft(Guid.NewGuid(), Build.Header(Guid.NewGuid(), true).Value, Build.Data());

        Assertions.FailsWith(result, InvoiceError.NotInDraft);
    }
}

public class TaxBreakdownTests
{
    [Fact]
    public void TaxBreakdown_WithoutData_IsEmpty()
    {
        var invoice = Build.DraftWith(Guid.NewGuid(), Build.Header(Guid.NewGuid(), false).Value, null);

        Assert.Empty(invoice.TaxBreakdown);
    }

    [Fact]
    public void TaxBreakdown_WithLines_MatchesBreakdownLineCreation()
    {
        var data = Build.Data(lines: 3);
        var invoice = Build.DraftWith(Guid.NewGuid(), Build.Header(Guid.NewGuid(), false).Value, data);

        var expected = BreakdownLine.CreateList(data.Lines);

        Assert.NotEmpty(invoice.TaxBreakdown);
        Assert.Equal(expected, invoice.TaxBreakdown);
    }

    [Fact]
    public void TaxBreakdown_ReflectsUpdatedData()
    {
        var invoice = Build.Draft();
        var newData = Build.Data(lines: 4);

        invoice.UpdateDraft(invoice.CustomerId, invoice.Header, newData);

        Assert.Equal(BreakdownLine.CreateList(newData.Lines), invoice.TaxBreakdown);
    }
}

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