using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public class OtherTaxIdTests
{
    [Theory]
    [InlineData("DE123456789", TaxIdType.IntraCommunityVatId, "DE")] 
    [InlineData("987654321", TaxIdType.Passport, "US")] 
    [InlineData("CA12345AB", TaxIdType.CountryOfOriginIdentityDocument, "IT")] 
    [InlineData("CR-2024-000123", TaxIdType.ResidenceCertificate, "FR")] 
    [InlineData("OTR-998877", TaxIdType.OtherSupportingDocument, "CH")] 
    public void Create_WithValidData_ReturnsSuccessfulOtherTaxId(string taxIdValue, 
                                                                TaxIdType taxIdType,
                                                                string taxIdCountryCode)
    {
        var result = OtherTaxId.Create(taxIdValue, taxIdType, taxIdCountryCode);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(taxIdValue, result.Value.Value);
        Assert.Equal(taxIdType, result.Value.Type);
        Assert.Equal(taxIdCountryCode, result.Value.CountryCode);
        Assert.Null(result.Value.IsLegalEntity);
    }

    [Theory]
    [InlineData("DE123456789000000000000000000000000000000000000000000000", TaxIdType.IntraCommunityVatId, "D")]
    [InlineData("DE123456789", TaxIdType.IntraCommunityVatId, "D")]
    [InlineData("987654321", TaxIdType.Passport, "ES")]
    [InlineData("12345678Z", TaxIdType.SpanishTaxId, "ES")]
    [InlineData(null, null, "")]
    public void Create_WithInvalidData_ReturnsFailure(string taxIdValue,
                                                    TaxIdType taxIdType,
                                                    string taxIdCountryCode)
    {
        var result = OtherTaxId.Create(taxIdValue, taxIdType, taxIdCountryCode);

        Assert.True(result.IsFailure);
        Assert.NotEmpty(result.Errors);
    }

    [Theory]
    [InlineData("HU")]
    [InlineData("ee")]
    [InlineData("  BE     ")]
    [InlineData("pL")]
    public void IsEuCountry_WithValidData_ReturnsTrue(string euCountryCode)
    {
        var result = TaxIdValidations.IsEuCountry(euCountryCode);
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("XX")]
    [InlineData("yy")]
    [InlineData("  Ñd     ")]
    public void IsEuCountry_WithInvalidData_ReturnsFalse(string euCountryCode)
    {
        var result = TaxIdValidations.IsEuCountry(euCountryCode);
        Assert.False(result);
    }
}