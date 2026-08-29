using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace Facturity.Modules.Customer.Tests.Domain;

public class ReconstruirClienteTests
{
    [Theory]
    [InlineData(2026, 08, 30, 07, 59, 59)]
    [InlineData(2026, 08, 30, 08, 00, 00)]
    [InlineData(2026, 07, 31, 02, 23, 53)]
    [InlineData(2025, 03, 15, 13, 43, 23)]
    public void ReconstruirCliente_DatosValidos_ClienteReconstruidoCorrectamente(int yyyy, int MM, int dd, int hh, int mm, int ss)
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(122).Value;
        const string alias = "MARCOS";
        var datosCliente = ReconstruirClienteTestsHelper.DatosClienteNif with
        {
            DeletedAtUtc = new DateTime(yyyy, MM, dd, hh, mm, ss, DateTimeKind.Utc)
        };
        
        var fakeCurrentTime = new DateTimeOffset(2026, 08, 30, 08, 00 ,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Cliente.Reconstruir(id, tenantId, alias, datosCliente.IdentificacionFiscal, datosCliente.RazonSocial, datosCliente.Nombre
            , datosCliente.PrimerApellido, datosCliente.SegundoApellido, datosCliente.Domicilio, datosCliente.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsSuccess);
    }
    
    [Theory]
    [InlineData(2026, 08, 30, 08, 00, 00)]
    [InlineData(2026, 07, 31, 08, 00, 00)]
    [InlineData(2026, 07, 30, 08, 00, 01)]
    public void ReconstruirCliente_DatoDeletedAtUtcInvalido_Fallo(int yyyy, int MM, int dd, int hh, int mm, int ss)
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(122).Value;
        const string alias = "MARCOS";
        var datosCliente = ReconstruirClienteTestsHelper.DatosClienteNif with
        {
            DeletedAtUtc = new DateTime(yyyy, MM, dd, hh, mm, ss, DateTimeKind.Utc)
        };
        var fakeCurrentTime = new DateTimeOffset(2026, 07, 30, 08, 00, 00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Cliente.Reconstruir(id, tenantId, alias, datosCliente.IdentificacionFiscal, datosCliente.RazonSocial, datosCliente.Nombre
            , datosCliente.PrimerApellido, datosCliente.SegundoApellido, datosCliente.Domicilio, datosCliente.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsFailure);
        result.Errors.Should().Contain(ClienteErrors.FechaFutura);
    }
}

public static class ReconstruirClienteTestsHelper
{
    public sealed record DatosReconstruirClienteRecord(
        IIdentificacionFiscal  IdentificacionFiscal,
        Domicilio? Domicilio,
        string? RazonSocial,
        string? Nombre,
        string? PrimerApellido,
        string? SegundoApellido,
        DateTime? DeletedAtUtc){}

    public static readonly DatosReconstruirClienteRecord DatosClienteNif = new(
        NifEspanol.Crear("47180975V").Value, 
        Domicilio.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
        null, 
        "Marcos Alexis", 
        "García", 
        "Machado",
        null
    );
}