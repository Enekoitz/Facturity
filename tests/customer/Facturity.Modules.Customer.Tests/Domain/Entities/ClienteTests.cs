using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Tests.Domain;

public class ClienteTests
{

    public static TheoryData<IIdentificacionFiscal, Domicilio, string?, string?, string?, string?> ObtenerObjetosParaPruebaCrearCliente()
    {
        var data = new TheoryData<IIdentificacionFiscal, Domicilio, string?, string?, string?, string?>
        {
            {
                NifEspanol.Crear("47180975V").Value,
                Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, null, "Marcos Alexis",
                "García", "Machado"
            },
            {
                NifEspanol.Crear("U89145882").Value,
                Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value,
                "Marcos Siderúrgicas S.L.", null, null, null
            },
            { IdOtroExtranjero.Crear("CR-2024-000123", TiposIdentificacionFiscal.CertificadoResidencia, "FR").Value, Domicilio.Crear("Calle Ramón y Cajal 23", "48001", "Bilbao", "Bizkaia", "País Vasco", "España").Value, "Marcos Siderúrgicas S.L.", null, null, null }
        };

        return data;
    }
    
    [Theory]
    [MemberData(nameof(ObtenerObjetosParaPruebaCrearCliente))]
    public void CrearCliente_ClienteValido_Exito(IIdentificacionFiscal identificacionFiscal, Domicilio domicilio, string? razonSocial, string? nombre, string? primerApellido, string? segundoApellido)
    {
        // Arrange
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(122).Value;
        string alias = "MARCOS";
        
        // Act
        var result = Cliente.Crear(id, tenantId, alias, identificacionFiscal, razonSocial, nombre, primerApellido, segundoApellido, domicilio);

        // Assert
        Assert.True(result.IsSuccess);
        
        Assert.Equal(result.Value.Id, id);
        Assert.Equal(result.Value.TenantId, tenantId);
        Assert.Equal(result.Value.Alias, alias);
        Assert.Equal(identificacionFiscal, result.Value.IdentificacionFiscal);
        Assert.Equal(domicilio, result.Value.Domicilio);
        Assert.Equal(razonSocial, result.Value.RazonSocial);
        Assert.Equal(nombre, result.Value.Nombre);
        Assert.Equal(primerApellido, result.Value.PrimerApellido);
        Assert.Equal(segundoApellido, result.Value.SegundoApellido);
        
        Assert.Empty(result.Errors);
    }
}
