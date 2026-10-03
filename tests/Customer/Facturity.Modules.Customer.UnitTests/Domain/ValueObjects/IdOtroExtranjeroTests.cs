using Facturity.Modules.Customer.Domain;
using Xunit;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public class IdOtroExtranjeroTests
{
    [Theory]
    [InlineData("DE123456789", TiposIdentificacionFiscal.NifIvaComunitario, "DE")] 
    [InlineData("987654321", TiposIdentificacionFiscal.Pasaporte, "US")] 
    [InlineData("CA12345AB", TiposIdentificacionFiscal.DocumentoIdentidadPaisOrigen, "IT")] 
    [InlineData("CR-2024-000123", TiposIdentificacionFiscal.CertificadoResidencia, "FR")] 
    [InlineData("OTR-998877", TiposIdentificacionFiscal.OtroDocumentoProbatorio, "CH")] 
    public void CrearIdOtroExtranjero_IdentificacionValida_Exito(string identificacionFiscalValor, 
                                                                TiposIdentificacionFiscal identificacionFiscalTipo,
                                                                string identificacionFiscalCodigoPais)
    {
        // Act
        var result = IdOtroExtranjero.Crear(identificacionFiscalValor, identificacionFiscalTipo, identificacionFiscalCodigoPais);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(identificacionFiscalValor, result.Value.Valor);
        Assert.Equal(identificacionFiscalTipo, result.Value.Tipo);
        Assert.Equal(identificacionFiscalCodigoPais, result.Value.CodigoPais);
        Assert.Null(result.Value.EsPersonaJuridica);
    }

    [Theory]
    [InlineData("DE123456789000000000000000000000000000000000000000000000", TiposIdentificacionFiscal.NifIvaComunitario, "D")]
    [InlineData("DE123456789", TiposIdentificacionFiscal.NifIvaComunitario, "D")]
    [InlineData("987654321", TiposIdentificacionFiscal.Pasaporte, "ES")]
    [InlineData("12345678Z", TiposIdentificacionFiscal.Nif, "ES")]
    [InlineData(null, null, "")]
    public void CrearIdOtroExtranjero_IdentificacionInvalida_Fallo(string identificacionFiscalValor,
                                                            TiposIdentificacionFiscal identificacionFiscalTipo,
                                                            string identificacionFiscalCodigoPais)
    {
        // Act
        var result = IdOtroExtranjero.Crear(identificacionFiscalValor, identificacionFiscalTipo, identificacionFiscalCodigoPais);

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
        var result = IdentificacionFiscalValidaciones.EsPaisUe(codigoPaisUe);

        // Assert
        Assert.True(result);
    }
}