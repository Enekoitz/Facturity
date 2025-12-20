using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Cliente.Servicios;
using UgeFactura.Domain.Cliente.ValueObjects;
using Xunit;

namespace UgeFactura.Domain.Tests.Cliente.Servicios
{
    public class TiposIdentificacionFiscalTests
    {
        [Theory]
        [InlineData("123")]
        [InlineData("   ")]
        [InlineData("xxx")]
        [InlineData(null)]
        public void ObtenerTipoIdentificacionFiscal_TipoCodigoVacio_RetornaError(string? tipoCodigoEntrada )
        {
            // Act
            var result = TiposIdentificacionFiscal.Obtener(tipoCodigoEntrada);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains("El código de tipo de identificación no es válido.", result.Errors[0].Message);
        }

        [Theory]
        [InlineData("01", "01", "NIF")]
        [InlineData("02", "02", "NIF-IVA")]
        [InlineData("03", "03", "Pasaporte")]
        [InlineData("04", "04", "Documento oficial de identificación expedido por el país o territorio de residencia")]
        [InlineData("05", "05", "Certificado de residencia")]
        [InlineData("06", "06", "Otro documento probatorio")]
        [InlineData("07", "07", "NIF Entidad extranjera")]
        public void ObtenerTipoIdentificacionFiscal_TipoCodigoExistente_RetornaResultadoCorrecto(string tipoCodigoEntrada, string tipoCodigoEsperado, string nombreEsperado)
        {

            // Act
            var result = TiposIdentificacionFiscal.Obtener(tipoCodigoEntrada);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(tipoCodigoEsperado, result.Value.Codigo);
            Assert.Equal(nombreEsperado, result.Value.Nombre);
        }
    }
}
