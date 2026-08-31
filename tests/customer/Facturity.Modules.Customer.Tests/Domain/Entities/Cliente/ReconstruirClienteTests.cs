using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Xunit.Sdk;

namespace Facturity.Modules.Customer.Tests.Domain;

public class ReconstruirClienteTests
{
    [Theory]
    [ClassData(typeof(ReconstruirClienteTestsObjetos))]
    public void ReconstruirCliente_DatosValidos_ClienteReconstruidoCorrectamente(
        ReconstruirClienteTestsHelper.DatosRecord datosCliente)
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(122).Value;
        const string alias = "MARCOS";
        
        // Posterior a todas las fechas del conjunto
        var fakeCurrentTime = new DateTimeOffset(2026, 11, 30, 08, 00 ,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Cliente.Reconstruir(
            id, tenantId, alias, datosCliente.IdentificacionFiscal, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, 
            datosCliente.Domicilio, datosCliente.ActivatedAtUtc, datosCliente.DeactivatedAtUtc, 
            datosCliente.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsSuccess);
    }
    
    [Theory]
    [ClassData(typeof(ReconstruirClienteTestsObjetos))]
    public void ReconstruirCliente_FechasDistintasAtUtcInvalido_Fallo(
        ReconstruirClienteTestsHelper.DatosRecord datosCliente)
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(122).Value;
        const string alias = "MARCOS";
        
        // Anterior a todas las fechas del conjunto
        var fakeCurrentTime = new DateTimeOffset(2026, 06, 12, 08, 43, 11, TimeSpan.Zero); 
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Cliente.Reconstruir(id, tenantId, alias, datosCliente.IdentificacionFiscal, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, 
            datosCliente.Domicilio, datosCliente.ActivatedAtUtc, datosCliente.DeactivatedAtUtc, 
            datosCliente.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsFailure);
        result.Errors.Should().Contain(ClienteErrors.FechaFutura);
    }
    
    [Fact]
    public void ReconstruirCliente_FechasActivacionDesactivacionSimultaneas_Fallo()
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(122).Value;
        const string alias = "MARCOS";
        
        var activatedAtUtc = new DateTimeOffset(2026, 05, 11, 05, 12, 23, TimeSpan.Zero);
        var deactivatedAtUtc = new DateTimeOffset(2026, 07, 12, 23, 43, 11, TimeSpan.Zero);

        var datosCliente = ReconstruirClienteTestsHelper.DatosClienteObjeto with
        {
            ActivatedAtUtc = activatedAtUtc,
            DeactivatedAtUtc = deactivatedAtUtc
        };
        
        // Anterior a todas las fechas del conjunto
        var fakeCurrentTime = new DateTimeOffset(2026, 06, 12, 08, 43, 11, TimeSpan.Zero); 
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Cliente.Reconstruir(id, tenantId, alias, datosCliente.IdentificacionFiscal, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, 
            datosCliente.Domicilio, datosCliente.ActivatedAtUtc, datosCliente.DeactivatedAtUtc, 
            datosCliente.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsFailure);
        result.Errors.Should().Contain(ClienteErrors.ActivacionDesactivacionSimultanea);
    }
}

public static class ReconstruirClienteTestsHelper
{
    public sealed record DatosRecord(
        IIdentificacionFiscal  IdentificacionFiscal,
        Domicilio? Domicilio,
        string? RazonSocial,
        string? Nombre,
        string? PrimerApellido,
        string? SegundoApellido,
        DateTimeOffset? ActivatedAtUtc,
        DateTimeOffset? DeactivatedAtUtc,
        DateTimeOffset? DeletedAtUtc
        ){}

    public static readonly DatosRecord DatosClienteObjeto = new(
        NifEspanol.Crear("47180975V").Value, 
        Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
        null, 
        "Marcos Alexis", 
        "García", 
        "Machado",
        null,
        new DateTimeOffset(2026, 08, 23, 08, 05, 45, TimeSpan.Zero),
        null
    );
}

public class ReconstruirClienteTestsObjetos : TheoryData<ReconstruirClienteTestsHelper.DatosRecord>
{
    public ReconstruirClienteTestsObjetos()
    {
        Add(ClienteConActivatedAtUtc);
        Add(ClienteConDeactivatedAtUtc);
        Add(ClienteConDeletedAtUtc);
    }
    
    public static readonly ReconstruirClienteTestsHelper.DatosRecord ClienteConActivatedAtUtc = 
        new(
            NifEspanol.Crear("47180975V").Value, 
            Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            new DateTimeOffset(2026, 08, 23, 08, 05, 45, TimeSpan.Zero),
            null,
            null
        );

    public static readonly ReconstruirClienteTestsHelper.DatosRecord ClienteConDeactivatedAtUtc = 
        new(
            NifEspanol.Crear("47180975V").Value, 
            Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            null,
            new DateTimeOffset(2026, 06, 12, 08, 43, 12, TimeSpan.Zero),
            null
        );
    
    public static readonly ReconstruirClienteTestsHelper.DatosRecord ClienteConDeletedAtUtc = 
        new(
            NifEspanol.Crear("47180975V").Value, 
            Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            null,
            null,
            new DateTimeOffset(2026, 07, 23, 21, 53, 00, TimeSpan.Zero)
        );
}