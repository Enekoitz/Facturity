using UgeFactura.Modules.Clientes.Domain;
namespace UgeFactura.Modules.Clientes.Tests.Domain;

public class IdentificacionFiscalTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    [InlineData("0344")]
    public void CrearIdentificacionFiscal_ConTipoCodigoInvalido_Fallo(string? tipoCodigoEntrada)
    {
        // Arrange
        string valor = "123456789";
        IdentificacionFiscalReglasEnum identificacionFiscalRegla = IdentificacionFiscalReglasEnum.Nif;

        // Act
        var result = IdentificacionFiscal.Crear(tipoCodigoEntrada, identificacionFiscalRegla, valor);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void CrearIdentificacionFiscal_ConValorInvalido_Fallo(string? valorEntrada)
    {
        // Arrange
        string tipoCodigo = "01";
        IdentificacionFiscalReglasEnum identificacionFiscalRegla = IdentificacionFiscalReglasEnum.Nif;

        // Act
        var result = IdentificacionFiscal.Crear(tipoCodigo, identificacionFiscalRegla, valorEntrada);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("E")]
    [InlineData("ESS")]
    public void CrearIdentificacionFiscal_CodigoPaisInvalido_Fallo(string? codigoPaisEntrada)
    {
        // Arrange
        string tipoCodigoIdentificacion = "04";
        string valorIdentificacion = "79269056Q";

        // Act
        var result = IdentificacionFiscal.Crear(tipoCodigoIdentificacion, valorIdentificacion, codigoPaisEntrada);

        // Assert
        Assert.True(result.IsFailure);
    }
}
