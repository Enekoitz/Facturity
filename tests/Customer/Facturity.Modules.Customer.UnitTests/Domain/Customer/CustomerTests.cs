using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public abstract class CustomerTests
{
    public class CrearClienteTests
    {
        [Theory]
        [ClassData(typeof(CreateCustomerTestsObjects))]
        public void Create_WithValidData_ReturnsCustomer(CreateCustomerTestsHelper.Data data)
        {
            string alias = "MARCOS";
            var id = Guid.NewGuid();
            var tenantId = TenantId.Create(Guid.NewGuid()).Value;
            
            var result = CreateCustomerTestsHelper.CreateCustomer(data, id, tenantId, alias);

            Assert.True(result.IsSuccess);
            Assert.Equal(id, result.Value.Id);
            Assert.Equal(tenantId, result.Value.TenantId);
            Assert.Equal(alias, result.Value.Alias);
            Assert.Equal(data.TaxId, result.Value.TaxId);
            Assert.Equal(data.Address, result.Value.Address);
            Assert.Equal(data.LegalEntityName, result.Value.LegalEntityName);
            Assert.Equal(data.Name, result.Value.Name);
            Assert.Equal(data.FirstSurname, result.Value.FirstSurname);
            Assert.Equal(data.SecondSurname, result.Value.SecondSurname);
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        [InlineData("Company name exceeds the maximum length. Company name exceeds the maximum length. Company name exceeds the maximum length. Company name exceeds the maximum length. Company name exceeds the maximum length. Company name exceeds the maximum length.Company name exceeds the maximum length. Company name exceeds the maximum length. Company name exceeds the maximum length. Company name exceeds the maximum length. Company name exceeds the maximum length. Company name exceeds the maximum length.")]
        public void Create_SpanishTaxIdAndLegalEntity_OnlyValidatesLegalEntityName_ReturnsFailure(string? legalEntityName)
        {
            var data = CreateCustomerTestsObjects.CustomerIsLegalEntity with
            {
                LegalEntityName = legalEntityName
            };

            var result =  CreateCustomerTestsHelper.CreateCustomer(data);
            
            Assert.True(result.IsFailure);

            result.Errors.Should().Contain(string.IsNullOrWhiteSpace(legalEntityName)
                ? CustomerErrors.LegalNameEmpty
                : CustomerErrors.LegalNameTooLong);

            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.NameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.SecondSurnameTooLong);
        }
        
        [Theory]
        [InlineData("", "")]
        [InlineData("     ", "       ")]
        [InlineData(null, null)]
        [InlineData("Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit.Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit. Name that exceeds the character limit.",
            "First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit.First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit. First surname that exceeds the character limit.")]
        public void  Create_SpanishTaxIdAndNotLegalEntity_OnlyValidatesNameAndSurnames_ReturnsFailure(string? name, string? firstSurname)
        {
            var data = CreateCustomerTestsObjects.CustomerNotIsLegalEntity with { Name = name, FirstSurname = firstSurname };

            var result = CreateCustomerTestsHelper.CreateCustomer(data);
            
            Assert.True(result.IsFailure);

            result.Errors.Should().Contain(string.IsNullOrWhiteSpace(name)
                ? CustomerErrors.NameEmpty
                : CustomerErrors.NameTooLong);
            
            result.Errors.Should().Contain(string.IsNullOrWhiteSpace(name)
                ? CustomerErrors.FirstSurnameEmpty
                : CustomerErrors.FirstSurnameTooLong);

            result.Errors.Should().NotContain(CustomerErrors.LegalNameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameTooLong);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void Create_InvalidDataWithoutLegalEntityNameNorNameAndSurname_ReturnsFailure(string? vacio)
        {
            var data =  CreateCustomerTestsObjects.CustomerOtherTaxId with
            { 
                LegalEntityName = vacio, Name = vacio, FirstSurname = vacio 
            };
            var result = CreateCustomerTestsHelper.CreateCustomer(data);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain(CustomerErrors.InvalidTaxDesignationType);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.NameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameTooLong);
        }

        [Fact]
        public void Create_WithLegalEntityName_OnlyValidatesLegalEntity_ReturnsFailure()
        {
            var data = CreateCustomerTestsObjects.CustomerIsLegalEntity with 
            { 
                LegalEntityName = Utilities.GetRandomLength(Parameters.CustomerParameters.LegalNameMaxLength + 1), 
                Name = null, 
                FirstSurname = null 
            };
            var result = CreateCustomerTestsHelper.CreateCustomer(data);

            result.Errors.Should().Contain(CustomerErrors.LegalNameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.NameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameTooLong);
        }

        [Fact]
        public void Create_WithNameAnsSurname_OnlyValidatesNameAndSurname_ReturnsFailure()
        {
            var datosCliente = CreateCustomerTestsObjects.CustomerNotIsLegalEntity with 
            { 
                LegalEntityName = null,
                Name = Utilities.GetRandomLength(Parameters.CustomerParameters.NameMaxLength + 1),
                FirstSurname = Utilities.GetRandomLength(Parameters.CustomerParameters.SurnameMaxLength + 1)
            };
            var result = CreateCustomerTestsHelper.CreateCustomer(datosCliente);

            result.Errors.Should().Contain(CustomerErrors.NameTooLong);
            result.Errors.Should().Contain(CustomerErrors.FirstSurnameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameTooLong);
        }
        
        [Fact]
        public void Create_WithInvalidSecondSurname_ReturnsFailure()
        {
            var datosCliente = CreateCustomerTestsObjects.CustomerNotIsLegalEntity with 
            { 
                LegalEntityName = null,
                Name = "Marcos",
                FirstSurname = "García",
                SecondSurname = Utilities.GetRandomLength(Parameters.CustomerParameters.SurnameMaxLength + 1)
            };
            var result = CreateCustomerTestsHelper.CreateCustomer(datosCliente);

            result.Errors.Should().Contain(CustomerErrors.SecondSurnameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameTooLong);
        }
    }

    public class ReconstruirClienteTests
{
    [Theory]
    [ClassData(typeof(RebuildCustomerTestsObjects))]
    public void Rebuild_WithValidData_ReturnsSuccessfulRebuildCustomer(
        RebuildCustomerTestsHelper.Data data)
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Create(Guid.NewGuid()).Value;
        const string alias = "MARCOS";
        
        var fakeCurrentTime = new DateTimeOffset(2026, 11, 30, 08, 00 ,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Customer.Domain.Customer.Rebuild(
            id, tenantId, alias, data.TaxId, data.LegalEntityName, 
            data.Name, data.FirstSurname, data.SecondSurname, 
            data.Address, data.ActivatedAtUtc, data.DeactivatedAtUtc, 
            data.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsSuccess);
    }
    
    [Theory]
    [ClassData(typeof(RebuildCustomerTestsObjects))]
    public void Rebuild_InvalidUtcDates_ReturnsFailure(
        RebuildCustomerTestsHelper.Data data)
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Create(Guid.NewGuid()).Value;
        const string alias = "MARCOS";
        
        var fakeCurrentTime = new DateTimeOffset(2026, 06, 12, 08, 43, 11, TimeSpan.Zero); 
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Customer.Domain.Customer.Rebuild(id, tenantId, alias, data.TaxId, data.LegalEntityName, 
            data.Name, data.FirstSurname, data.SecondSurname, 
            data.Address, data.ActivatedAtUtc, data.DeactivatedAtUtc, 
            data.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsFailure);
        result.Errors.Should().Contain(CustomerErrors.InvalidFutureDate);
    }
    
    [Fact]
    public void Rebuild_BothActivationAndDeactivationDatesProvided_ReturnsFailure()
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Create(Guid.NewGuid()).Value;
        const string alias = "MARCOS";
        
        var activatedAtUtc = new DateTimeOffset(2026, 05, 11, 05, 12, 23, TimeSpan.Zero);
        var deactivatedAtUtc = new DateTimeOffset(2026, 07, 12, 23, 43, 11, TimeSpan.Zero);

        var data = RebuildCustomerTestsHelper.DataObject with
        {
            ActivatedAtUtc = activatedAtUtc,
            DeactivatedAtUtc = deactivatedAtUtc
        };
        
        var fakeCurrentTime = new DateTimeOffset(2026, 06, 12, 08, 43, 11, TimeSpan.Zero); 
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Customer.Domain.Customer.Rebuild(id, tenantId, alias, data.TaxId, data.LegalEntityName, 
            data.Name, data.FirstSurname, data.SecondSurname, 
            data.Address, data.ActivatedAtUtc, data.DeactivatedAtUtc, 
            data.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsFailure);
        result.Errors.Should().Contain(CustomerErrors.SimultaneousActivationDeactivationDate);
    }
}
}