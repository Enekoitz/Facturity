using UgeFactura.Modules.Clientes.Domain;

namespace UgeFactura.Modules.Clients.Tests.ValueObjects
{
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
            Assert.Single(result.Error);
            Assert.Equal(CodigoPostalCodigosError.CodigoPostal_Vacio.ToString(), result.Error);
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
            Assert.Single(result.Error);
            Assert.Equal(CodigoPostalCodigosError.CodigoPostal_FormatoInvalido.ToString(), result.Error);
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
            Assert.Single(result.Error);
            Assert.Equal(CodigoPostalCodigosError.CodigoPostal_LongitudInvalida.ToString(), result.Error);
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
            Assert.Empty(result.Error);
            Assert.Equal(cp, result.Value.ToString());
        }
    }
}