using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Clientes;
using Xunit;

namespace UgeFactura.Domain.Tests.Cliente.ValueObjects
{
    //public class IdentificacionFiscalTests
    //{
    //    [Theory]
    //    [InlineData(null)]
    //    [InlineData("  ")]
    //    [InlineData("")]
    //    public void CrearIdentificacionFiscal_ConTipoCodigoVacio_RetornaResultadoErroneo(string? tipoCodigoEntrada)
    //    {
    //        // Arrange
    //        string valor = "123456789";

    //        // Act
    //        var result = IdentificacionFiscal.Crear(tipoCodigoEntrada, valor);

    //        // Assert
    //        Assert.True(result.IsFailed);
    //        Assert.Contains("El código de tipo de identificación no puede estar vacío.", result.Errors[0].Message);
    //    }

    //    [Theory]
    //    [InlineData(null)]
    //    [InlineData("  ")]
    //    [InlineData("")]
    //    public void CrearIdentificacionFiscal_ConValorCodigoVacio_RetornaResultadoCorrecto(string? valorEntrada)
    //    {
    //        // Arrange
    //        string tipoCodigo = "01";

    //        // Act
    //        var result = IdentificacionFiscal.Crear(tipoCodigo, valorEntrada);

    //        // Assert
    //        Assert.True(result.IsFailed);
    //        Assert.Contains("El valor de la identificación no puede estar vacío.", result.Errors[0].Message);
    //    }

    //    [Theory]
    //    [InlineData(null)]
    //    [InlineData("  ")]
    //    [InlineData("")]
    //    [InlineData("0344")]
    //    public void CrearIdentificacionFiscal_ConTipoCodigoInvalidos_RetornaResultadoErroneo(string? tipoCodigoEntrada)
    //    {
    //        // Act
    //        var result = TiposIdentificacionFiscal.Obtener(tipoCodigoEntrada);

    //        // Assert
    //        Assert.True(result.IsFailed);
    //        Assert.Contains("El código de tipo de identificación no es válido.", result.Errors[0].Message);
    //    }

    //    [Theory]
    //    [InlineData(null)]
    //    [InlineData("")]
    //    [InlineData("  ")]
    //    [InlineData("E")]
    //    [InlineData("ESS")]
    //    public void CrearIdentificacionFiscal_CodigoPaisInvalido_RetornaResultadoErroneo(string? codigoPaisEntrada)
    //    {
    //        // Arrange
    //        string tipoCodigoIdentificacion = "04";
    //        string valorIdentificacion = "79269056Q";

    //        // Act
    //        var result = IdentificacionFiscal.Crear(tipoCodigoIdentificacion, valorIdentificacion, codigoPaisEntrada);

    //        // Assert
    //        Assert.True(result.IsFailed);
    //    }
    //}
}
