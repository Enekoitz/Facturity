using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Cliente.ValueObjects;
using Xunit;

namespace UgeFactura.Domain.Tests.Cliente.ValueObjects
{
    public class CodigoPostalTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void CrearCodigoPostal_CodigoPostalVacio_Fallo(string? codigoPostalEntrada)
        {

            // Act
            var result = CodigoPostal.Crear(codigoPostalEntrada);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal("El código postal no puede estar vacío.", result.Errors[0].Message);
        }

        [Fact]
        public void CrearCodigoPostal_CodigoPostalNoSoloDigitos_Fallo()
        {
            // Arrange
            string cp = "12f&3";

            // Act
            var result = CodigoPostal.Crear(cp);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal("El código postal debe contener solo dígitos.", result.Errors[0].Message);
        }

        [Fact]
        public void CrearCodigoPostal_CodigoPostalMasDe5Caracteres_Fallo()
        {
            // Arrange
            string cp = "123456";

            // Act
            var result = CodigoPostal.Crear(cp);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal("El código postal debe tener una longitud de 5 dígitos.", result.Errors[0].Message);
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
            Assert.Equal(cp, result.Value.ToString());
        }
    }
}