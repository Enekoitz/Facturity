using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UgeFactura.Domain.Comun.Mensajes;
using Xunit;
using Xunit.Sdk;

namespace UgeFactura.Domain.Comun.Validaciones
{
    public class ValidarIdentificacionFiscalUtilsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void EsNifValido_NIF_RetornaResultadoErroneo(string? codigoPaisEsperado)
        {
            // Act
            var esValido = ValidarCodigos.EsCodigoPaisValido(codigoPaisEsperado, out string codigoError);

            // Assert
            Assert.False(esValido);
            Assert.Equal(IdentificacionFiscalCodigosError.CodigoPais_Vacio.ToString(), codigoError);
        }

        [Theory]
        [InlineData("E")]
        [InlineData("ESS")]
        public void EsCodigoPais_CodigoPaisLongitudInvalida_RetornaResultadoErroneo(string codigoPaisEsperado)
        {
            // Act
            var esValido = ValidarCodigos.EsCodigoPaisValido(codigoPaisEsperado, out string codigoError);

            // Assert
            Assert.False(esValido);
            Assert.Equal(IdentificacionFiscalCodigosError.CodigoPais_LongitudInvalida.ToString(), codigoError);
        }

        [Theory]
        [InlineData("ES")]
        [InlineData("FR")]
        [InlineData("IT")]
        public void EsCodigoPais_CodigoPaisValido_RetornaResultadoCorrectO(string codigoPaisEsperado)
        {
            // Act
            var esValido = ValidarCodigos.EsCodigoPaisValido(codigoPaisEsperado, out string codigoError);

            // Assert
            Assert.True(esValido);
        }
    }
}
