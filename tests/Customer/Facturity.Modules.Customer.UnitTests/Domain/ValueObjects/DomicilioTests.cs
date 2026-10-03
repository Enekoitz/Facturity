using Facturity.Modules.Customer.Domain;
using Xunit;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public class DomicilioTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void CrearDomicilio_ConDireccionInvalida_Fallo(string direccion)
    {
        //Arrange
        string? cp = "48001";
        string? municipio = "Bilbao";
        string? provincia = null;
        string? region = null;
        string? pais = null;

        // Act
        var result = Domicilio.Crear(direccion, cp, municipio, provincia, region, pais);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CrearDomicilio_ParametrosRequeridosCorrectos_Exito()
    {
        //Arrange
        string direccion = "Calle Esperanza 12º";
        string? cp = "48001";
        string? municipio = "Bilbao";
        string? provincia = null;
        string? region = null;
        string? pais = null;

        // Act
        var result = Domicilio.Crear(direccion, cp, municipio, provincia, region, pais);

        // Assert
        Assert.True(result.IsSuccess);
    }
}