using Facturity.Modules.Customer.Domain;
using Xunit;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public class SpanishTaxIdTests
{
    [Theory]
    [InlineData("12345678Z")] // DNI
    [InlineData("X1234567L")] // NIE
    [InlineData("A12345674")] // CIF
    public void CrearNifEspanol_ValorNifValido_Exito(string valorNif)
    {
        // Act
        var result = SpanishTaxId.Crear(valorNif);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(valorNif, result.Value.Valor);
    }
    
    [Theory]
    [InlineData("A12345674")] // CIF
    public void CrearNifEspanol_TipoCif_DebeSerPersonaJuridica_Exito(string valorNif)
    {
        // Act
        var result = SpanishTaxId.Crear(valorNif);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(true, result.Value.IsLegalEntity);
    }
    
    [Theory]
    [InlineData("12345678Z")] // DNI
    [InlineData("X1234567L")] // NIE
    public void CrearNifEspanol_TipoNifNie_NoDebeSerPersonaJuridica_Exito(string valorNif)
    {
        // Act
        var result = SpanishTaxId.Crear(valorNif);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(false, result.Value.IsLegalEntity);
    }

    [Theory]
    [InlineData("11111678Z")] 
    [InlineData("X12F4567L")]
    [InlineData("ccccccccc")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearNifEspanol_ValorNifInvalido_Fallo(string valorNif)
    {
        // Act
        var result = SpanishTaxId.Crear(valorNif);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEmpty(result.Errors);
    }
}