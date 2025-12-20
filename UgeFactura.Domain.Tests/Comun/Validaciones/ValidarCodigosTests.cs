using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace UgeFactura.Domain.Comun.Validaciones
{
    public class ValidarCodigosTests
    {
        private readonly string MensajeErrorCodigoPaisVacio = "El código de país no puede estar vacío." ;
        private readonly string MensajeErrorCodigoPaisLongitudInvalida = "El código de país debe tener una longitud de 2 caracteres.";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ValidarEsCodigoPais_CodigoPaisVacio_RetornaResultadoErroneo(string? codigoPaisEsperado)
        {
            // Act
            var esValido = ValidarCodigos.EsCodigoPaisValido(codigoPaisEsperado, out string mensajeError);

            // Assert
            Assert.False(esValido);
            Assert.Equal(MensajeErrorCodigoPaisVacio, mensajeError);
        }

        [Theory]
        [InlineData("E")]
        [InlineData("ESS")]
        public void ValidarEsCodigoPais_CodigoPaisLongitudInvalida_RetornaResultadoErroneo(string codigoPaisEsperado)
        {
            // Act
            var esValido = ValidarCodigos.EsCodigoPaisValido(codigoPaisEsperado, out string mensajeError);

            // Assert
            Assert.False(esValido);
            Assert.Equal(MensajeErrorCodigoPaisLongitudInvalida, mensajeError);
        }

        [Theory]
        [InlineData("ES")]
        [InlineData("FR")]
        [InlineData("IT")]
        public void ValidarEsCodigoPais_CodigoPaisValido_RetornaResultadoCorrectO(string codigoPaisEsperado)
        {
            // Act
            var esValido = ValidarCodigos.EsCodigoPaisValido(codigoPaisEsperado, out string mensajeError);

            // Assert
            Assert.True(esValido);
        }
    }
}
