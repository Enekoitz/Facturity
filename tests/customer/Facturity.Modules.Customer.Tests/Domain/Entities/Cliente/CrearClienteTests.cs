using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using FluentAssertions;

namespace Facturity.Modules.Customer.Tests.Domain;

public class CrearClienteTests
{
    [Theory]
    [ClassData(typeof(CrearClienteTestsObjetos))]
    public void CrearCliente_DatosValidos_ClienteCreadoCorrectamente(CrearClienteTestsHelper.DatosCrearClienteRecord datosCrearClienteRecord)
    {
        string alias = "MARCOS";
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(122).Value;
        
        var result = CrearClienteTestsHelper.CrearCliente(datosCrearClienteRecord, id, tenantId, alias);

        Assert.True(result.IsSuccess);
        
        Assert.Equal(id, result.Value.Id);
        Assert.Equal(tenantId, result.Value.TenantId);
        Assert.Equal(alias, result.Value.Alias);
        Assert.Equal(datosCrearClienteRecord.IdentificacionFiscal, result.Value.IdentificacionFiscal);
        Assert.Equal(datosCrearClienteRecord.Domicilio, result.Value.Domicilio);
        Assert.Equal(datosCrearClienteRecord.RazonSocial, result.Value.RazonSocial);
        Assert.Equal(datosCrearClienteRecord.Nombre, result.Value.Nombre);
        Assert.Equal(datosCrearClienteRecord.PrimerApellido, result.Value.PrimerApellido);
        Assert.Equal(datosCrearClienteRecord.SegundoApellido, result.Value.SegundoApellido);
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    [InlineData("Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. ")]
    public void CrearCliente_NifEspañol_EsPersonaJuridica_SoloValidaRazonSocial(string? razonSocialNuloVacio)
    {
        var datosCliente = CrearClienteTestsObjetos.ClienteNifPersonaJuridica1 with
        {
            RazonSocial = razonSocialNuloVacio
        };

        var result =  CrearClienteTestsHelper.CrearCliente(datosCliente);
        
        Assert.True(result.IsFailure);

        result.Errors.Should().Contain(string.IsNullOrWhiteSpace(razonSocialNuloVacio)
            ? ClienteErrors.RazonSocialVacio
            : ClienteErrors.RazonSocialDemasiadoLargo);

        result.Errors.Should().NotContain(ClienteErrors.NombreVacio);
        result.Errors.Should().NotContain(ClienteErrors.NombreDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoVacio);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.SegundoApellidoDemasiadoLargo);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("     ", "       ")]
    [InlineData(null, null)]
    [InlineData("Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. ",
        "Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. ")]
    public void CrearCliente_NifEspañol_NoEsPersonaJuridica_SoloValidaNombreYApellido(string? nombre, string? primerApellido)
    {
        var datosCliente = CrearClienteTestsObjetos.ClienteNifNoPersonaJuridica1 with { Nombre = nombre, PrimerApellido = primerApellido };

        var result = CrearClienteTestsHelper.CrearCliente(datosCliente);
        
        Assert.True(result.IsFailure);

        result.Errors.Should().Contain(string.IsNullOrWhiteSpace(nombre)
            ? ClienteErrors.NombreVacio
            : ClienteErrors.NombreDemasiadoLargo);
        
        result.Errors.Should().Contain(string.IsNullOrWhiteSpace(nombre)
            ? ClienteErrors.PrimerApellidoVacio
            : ClienteErrors.PrimerApellidoDemasiadoLargo);

        result.Errors.Should().NotContain(ClienteErrors.RazonSocialVacio);
        result.Errors.Should().NotContain(ClienteErrors.RazonSocialDemasiadoLargo);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void CrearCliente_NingunoInformado_GeneraErrorNiRazonSocialNiNombre(string? vacio)
    {
        var datosCliente =  CrearClienteTestsObjetos.ClienteIdOtroCertificadoResidencia1 with
        { 
            RazonSocial = vacio, Nombre = vacio, PrimerApellido = vacio 
        };
        var result = CrearClienteTestsHelper.CrearCliente(datosCliente);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(ClienteErrors.NiRazonSocialNiNombreApellido);
        result.Errors.Should().NotContain(ClienteErrors.RazonSocialVacio);
        result.Errors.Should().NotContain(ClienteErrors.RazonSocialDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.NombreVacio);
        result.Errors.Should().NotContain(ClienteErrors.NombreDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoVacio);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoDemasiadoLargo);
    }

    [Fact]
    public void CrearCliente_SoloRazonSocialInformada_ValidaSoloRazonSocial()
    {
        var datosCliente = CrearClienteTestsObjetos.ClienteNifPersonaJuridica1 with 
        { 
            RazonSocial = Utilities.GetRandomLength(ClienteParametros.RazonSocialMaxLength + 1), 
            Nombre = null, 
            PrimerApellido = null 
        };
        var result = CrearClienteTestsHelper.CrearCliente(datosCliente);

        result.Errors.Should().Contain(ClienteErrors.RazonSocialDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.NombreVacio);
        result.Errors.Should().NotContain(ClienteErrors.NombreDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoVacio);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoDemasiadoLargo);
    }

    [Fact]
    public void CrearCliente_SoloNombreInformado_ValidaSoloNombreYApellido()
    {
        var datosCliente = CrearClienteTestsObjetos.ClienteNifNoPersonaJuridica1 with 
        { 
            RazonSocial = null,
            Nombre = Utilities.GetRandomLength(ClienteParametros.NombreMaxLength + 1),
            PrimerApellido = Utilities.GetRandomLength(ClienteParametros.ApellidoMaxLength + 1)
        };
        var result = CrearClienteTestsHelper.CrearCliente(datosCliente);

        result.Errors.Should().Contain(ClienteErrors.NombreDemasiadoLargo);
        result.Errors.Should().Contain(ClienteErrors.PrimerApellidoDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.NombreVacio);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoVacio);
        result.Errors.Should().NotContain(ClienteErrors.RazonSocialVacio);
        result.Errors.Should().NotContain(ClienteErrors.RazonSocialDemasiadoLargo);
    }
    
    [Fact]
    public void CrearCliente_ConSegundoApellidoInformadoSuperaLongitudMaxima_Fallo()
    {
        var datosCliente = CrearClienteTestsObjetos.ClienteNifNoPersonaJuridica1 with 
        { 
            RazonSocial = null,
            Nombre = "Marcos",
            PrimerApellido = "García",
            SegundoApellido = Utilities.GetRandomLength(ClienteParametros.ApellidoMaxLength + 1)
        };
        var result = CrearClienteTestsHelper.CrearCliente(datosCliente);

        result.Errors.Should().Contain(ClienteErrors.SegundoApellidoDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.NombreDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoDemasiadoLargo);
        result.Errors.Should().NotContain(ClienteErrors.NombreVacio);
        result.Errors.Should().NotContain(ClienteErrors.PrimerApellidoVacio);
        result.Errors.Should().NotContain(ClienteErrors.RazonSocialVacio);
        result.Errors.Should().NotContain(ClienteErrors.RazonSocialDemasiadoLargo);
    }
}

public static class CrearClienteTestsHelper
{
    public sealed record DatosCrearClienteRecord(
        IIdentificacionFiscal  IdentificacionFiscal,
        Domicilio? Domicilio,
        string? RazonSocial,
        string? Nombre,
        string? PrimerApellido,
        string? SegundoApellido){}
    
    public static Result<Cliente> CrearCliente(DatosCrearClienteRecord datosCliente, in Guid id , in TenantId tenantId, in string alias)
    {
        var result = Cliente.Crear(id, tenantId, alias, datosCliente.IdentificacionFiscal, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, datosCliente.Domicilio);

        return result;
    }
    
    public static Result<Cliente> CrearCliente(DatosCrearClienteRecord datosCliente, string alias = "MARCOS")
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(122).Value;
        
        var result = Cliente.Crear(id, tenantId, alias, datosCliente.IdentificacionFiscal, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, datosCliente.Domicilio);

        return result;
    }
}

public class CrearClienteTestsObjetos : TheoryData<CrearClienteTestsHelper.DatosCrearClienteRecord>
{
    public CrearClienteTestsObjetos()
    {
        Add(ClienteNifNoPersonaJuridica1);
            
        Add(ClienteNifPersonaJuridica1);
            
        Add(ClienteIdOtroCertificadoResidencia1);
            
        Add(new CrearClienteTestsHelper.DatosCrearClienteRecord(IdOtroExtranjero.Crear("CR-2024-000123", TiposIdentificacionFiscal.CertificadoResidencia,
                "FR").Value, null, "Marcos Siderúrgicas S.L.", null, null, null
        ));
    }
    
    public static readonly CrearClienteTestsHelper.DatosCrearClienteRecord ClienteNifNoPersonaJuridica1 = new (
        NifEspanol.Crear("47180975V").Value,
        Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null,
            null, null).Value, null, "Marcos Alexis", "García", "Machado"
    );

    public static readonly CrearClienteTestsHelper.DatosCrearClienteRecord ClienteNifPersonaJuridica1 = new (
        NifEspanol.Crear("U89145882").Value,
        Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null,
            null, null).Value,
        "Marcos Siderúrgicas S.L.", null, null, null
    );
    
    public static readonly CrearClienteTestsHelper.DatosCrearClienteRecord ClienteIdOtroCertificadoResidencia1 = new (IdOtroExtranjero.Crear("CR-2024-000123", TiposIdentificacionFiscal.CertificadoResidencia,
            "FR").Value, Domicilio.Crear("Calle Ramón y Cajal 23", "48001", "Bilbao",
            "Bizkaia", "País Vasco", "España").Value, "Marcos Siderúrgicas S.L.", null, null, null
    );
}
