using Facturity.Modules.Customer.Domain;
using Xunit;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public class OtherTaxIdTests
{
    [Theory]
    [InlineData("DE123456789", TaxIdType.IntraCommunityVatId, "DE")] 
    [InlineData("987654321", TaxIdType.Passport, "US")] 
    [InlineData("CA12345AB", TaxIdType.CountryOfOriginIdentityDocument, "IT")] 
    [InlineData("CR-2024-000123", TaxIdType.ResidenceCertificate, "FR")] 
    [InlineData("OTR-998877", TaxIdType.OtherSupportingDocument, "CH")] 
    public void CrearIdOtroExtranjero_IdentificacionValida_Exito(string identificacionFiscalValor, 
                                                                TaxIdType identificacionFiscalTipo,
                                                                string identificacionFiscalCodigoPais)
    {
        // Act
        var result = OtherTaxId.Create(identificacionFiscalValor, identificacionFiscalTipo, identificacionFiscalCodigoPais);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(identificacionFiscalValor, result.Value.Value);
        Assert.Equal(identificacionFiscalTipo, result.Value.Type);
        Assert.Equal(identificacionFiscalCodigoPais, result.Value.CountryCode);
        Assert.Null(result.Value.IsLegalEntity);
    }

    [Theory]
    [InlineData("DE123456789000000000000000000000000000000000000000000000", TaxIdType.IntraCommunityVatId, "D")]
    [InlineData("DE123456789", TaxIdType.IntraCommunityVatId, "D")]
    [InlineData("987654321", TaxIdType.Passport, "ES")]
    [InlineData("12345678Z", TaxIdType.TaxId, "ES")]
    [InlineData(null, null, "")]
    public void CrearIdOtroExtranjero_IdentificacionInvalida_Fallo(string identificacionFiscalValor,
                                                            TaxIdType identificacionFiscalTipo,
                                                            string identificacionFiscalCodigoPais)
    {
        // Act
        var result = OtherTaxId.Create(identificacionFiscalValor, identificacionFiscalTipo, identificacionFiscalCodigoPais);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEmpty(result.Errors);
    }

    [Theory]
    [InlineData("HU")]
    [InlineData("ee")]
    [InlineData("  BE     ")]
    [InlineData("pL")]
    public void EsPaisUE_EsCodigoPaisISO2UEValido_Fallo(string codigoPaisUe)
    {
        // Act
        var result = TaxIdValidations.IsEuCountry(codigoPaisUe);

        // Assert
        Assert.True(result);
    }
}