using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Tests.Domain;

public class ClienteTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void CrearCliente_E_Fallo(string codigoPostalEntrada)
    {

        // Act
        var result = CodigoPostal.Crear(codigoPostalEntrada);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(CodigoPostalCodigosError.CodigoPostal_Vacio.ToString(), result.Errors[0].Code.ToString());
    }
}
