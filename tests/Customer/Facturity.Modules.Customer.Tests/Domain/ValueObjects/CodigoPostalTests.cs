using Facturity.Modules.Customer.Domain;
namespace Facturity.Modules.Customer.Tests.Domain;

public class CodigoPostalTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void CrearCodigoPostal_CodigoPostalVacio_Fallo(string codigoPostalEntrada)
    {

        // Act
        var result = CodigoPostal.Crear(codigoPostalEntrada);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void CrearCodigoPostal_CodigoPostalNoSoloDigitos_Fallo()
    {
        // Arrange
        string cp = "12f&3";

        // Act
        var result = CodigoPostal.Crear(cp);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void CrearCodigoPostal_CodigoPostalMasDe5Caracteres_Fallo()
    {
        // Arrange
        string cp = "123456";

        // Act
        var result = CodigoPostal.Crear(cp);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void CrearCodigoPostal_CodigoPostalCorrecto_Exito()
    {
        // Arrange
        string cp = "02344";

        // Act
        var result = CodigoPostal.Crear(cp);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(cp, result.Value.Valor);
    }
}