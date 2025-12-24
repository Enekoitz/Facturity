using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Cliente.ValueObjects;
using UgeFactura.Domain.Comun.Mensajes;
using Xunit;
using Xunit.Sdk;

namespace UgeFactura.Domain.Comun.Validaciones
{
    public class ValidarCodigosTests
    {
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
            Assert.Equal(IdentificacionFiscalCodigosError.CodigoPais_Vacio.ToString(), mensajeError);
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
            Assert.Equal(IdentificacionFiscalCodigosError.CodigoPais_LongitudInvalida.ToString(), mensajeError);
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
