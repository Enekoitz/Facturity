using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Clientes;
using UgeFactura.Domain.Comun;
using Xunit;

namespace UgeFactura.Domain.Tests.Cliente.Servicios
{
    public class SubTiposIdentificacionFiscalTests
    {
        //[Theory]
        //[InlineData("   ")]
        //[InlineData("")]
        //public void ObtenerSubTipo_TipoCodigoVacio_RetornaError(string tipoCodigoEntrada )
        //{
        //    // Arrange
        //    string valorIdentificacion = "08933709A";

        //    // Act
        //    var result = SubTiposIdentificacionFiscal.ObtenerSubTipo(tipoCodigoEntrada, valorIdentificacion);

        //    // Assert
        //    Assert.True(result.IsFailed);
        //    Assert.Contains(IdentificacionFiscalCodigosError.TipoIdentificacionFiscal_Vacio.ToString(), result.Errors[0].Message);
        //}

        //[Theory]
        //[InlineData("   ")]
        //[InlineData("")]
        //public void ObtenerSubTipo_ValorVacio_RetornaError(string valorIdentificacionEntrada)
        //{
        //    // Arrange
        //    string tipoCodigo = "01";

        //    // Act
        //    var result = SubTiposIdentificacionFiscal.ObtenerSubTipo(tipoCodigo, valorIdentificacionEntrada);

        //    // Assert
        //    Assert.True(result.IsFailed);
        //    Assert.Contains(IdentificacionFiscalCodigosError.ValorIdentificacionFiscal_Vacio.ToString(), result.Errors[0].Message);
        //}

        //[Fact]
        //public void ObtenerSubTipo_NIFValido_RetornaResultadoCorrecto()
        //{
        //    // Arrange
        //    string tipoCodigo = "01";
        //    string valorIdentificacion = "08933709A";

        //    // Act
        //    var result = SubTiposIdentificacionFiscal.ObtenerSubTipo(tipoCodigo, valorIdentificacion);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.Equal(SubTipoIdentificacionFiscal.NifPersonaFisica, result.Value);
        //}

        //[Fact]
        //public void ObtenerSubTipo_NIEValido_RetornaResultadoCorrecto()
        //{
        //    // Arrange
        //    string tipoCodigo = "01";
        //    string valorIdentificacion = "Y8295847Z";

        //    // Act
        //    var result = SubTiposIdentificacionFiscal.ObtenerSubTipo(tipoCodigo, valorIdentificacion);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.Equal(SubTipoIdentificacionFiscal.Nie, result.Value);
        //}

        //[Fact]
        //public void ObtenerSubTipo_CIFValido_RetornaResultadoCorrecto()
        //{
        //    // Arrange
        //    string tipoCodigo = "01";
        //    string valorIdentificacion = "R3874547G";

        //    // Act
        //    var result = SubTiposIdentificacionFiscal.ObtenerSubTipo(tipoCodigo, valorIdentificacion);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.Equal(SubTipoIdentificacionFiscal.NifPersonaJuridica, result.Value);
        //}

        //[Fact]
        //public void ObtenerSubTipo_Tipo01ValorInvalido_RetornaResultadoCorrecto()
        //{
        //    // Arrange
        //    string tipoCodigo = "01";
        //    string valorIdentificacion = "R387D347G";

        //    // Act
        //    var result = SubTiposIdentificacionFiscal.ObtenerSubTipo(tipoCodigo, valorIdentificacion);

        //    // Assert
        //    Assert.True(result.IsFailed);
        //    Assert.Contains(result.Errors, e => e.Message == IdentificacionFiscalCodigosError.SubTipoIdentificacionFiscal_Invalido.ToString());
        //}

        //[Fact]
        //public void ObtenerSubTipo_TipoDiferenteAl01_RetornaResultadoCorrectoNull()
        //{
        //    // Arrange
        //    string tipoCodigo = "03";
        //    string valorIdentificacion = "Y8295847Z";

        //    // Act
        //    var result = SubTiposIdentificacionFiscal.ObtenerSubTipo(tipoCodigo, valorIdentificacion);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.Null(result.Value);
        //}
    }
}
