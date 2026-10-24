using System;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;
using Xunit;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public static class ActualizarClienteTestsHelper
{
    public sealed record DatosRecord(
        string Alias,
        ITaxId  TaxId,
        string? RazonSocial,
        string? Nombre,
        string? PrimerApellido,
        string? SegundoApellido,
        Address? Domicilio
    ){}
}

public class ActualizarClienteTestsObjetos 
    : TheoryData<ActualizarClienteTestsHelper.DatosRecord, ActualizarClienteTestsHelper.DatosRecord>
{
    public ActualizarClienteTestsObjetos()
    {
        Add(Cliente1, Cliente2);
        Add(Cliente2, Cliente3);
        Add(Cliente3, Cliente1);
    }

    public static readonly ActualizarClienteTestsHelper.DatosRecord Cliente1 = 
        new(
            "MARC",
            SpanishTaxId.Crear("47180975V").Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            Address.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value
        );

    public static readonly ActualizarClienteTestsHelper.DatosRecord Cliente2 = 
        new(
            "FTNPARK",
            SpanishTaxId.Crear("A31497027").Value, 
            "FitnessPark", 
            null, 
            null, 
            null,
            Address.Crear("Polígono Industrial Lapatza, Aldatza Kalea, 2", "48970", "Basauri", "Bizkaia", "Euskadi", "España").Value
        );

    public static readonly ActualizarClienteTestsHelper.DatosRecord Cliente3 = 
        new(
            "FRANC",
            OtherTaxId.Create("CR-2024-000123", 
                TaxIdType.ResidenceCertificate,
                "FR").Value, 
            "Francais S.A.", 
            null, 
            null, 
            null,
            Address.Crear("33, Chemin de Saugane", "12290", "Prades-Salars", "Aveyron", "Occitanie", "France").Value
        );
}

public static class ReconstruirClienteTestsHelper
{
    public sealed record DatosRecord(
        ITaxId  TaxId,
        Address? Domicilio,
        string? RazonSocial,
        string? Nombre,
        string? PrimerApellido,
        string? SegundoApellido,
        DateTimeOffset? ActivatedAtUtc,
        DateTimeOffset? DeactivatedAtUtc,
        DateTimeOffset? DeletedAtUtc
        ){}

    public static readonly DatosRecord DatosClienteObjeto = new(
        SpanishTaxId.Crear("47180975V").Value, 
        Address.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
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
            SpanishTaxId.Crear("47180975V").Value, 
            Address.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
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
            SpanishTaxId.Crear("47180975V").Value, 
            Address.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
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
            SpanishTaxId.Crear("47180975V").Value, 
            Address.Crear("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            null,
            null,
            new DateTimeOffset(2026, 07, 23, 21, 53, 00, TimeSpan.Zero)
        );
}

public static class CrearClienteTestsHelper
{
    public sealed record DatosCrearClienteRecord(
        ITaxId  TaxId,
        Address? Domicilio,
        string? RazonSocial,
        string? Nombre,
        string? PrimerApellido,
        string? SegundoApellido){}
    
    public static Result<Customer.Domain.Customer> CrearCliente(DatosCrearClienteRecord datosCliente, in Guid id , in TenantId tenantId, in string alias)
    {
        var result = Customer.Domain.Customer.Crear(id, tenantId, alias, datosCliente.TaxId, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, datosCliente.Domicilio);

        return result;
    }
    
    public static Result<Customer.Domain.Customer> CrearCliente(DatosCrearClienteRecord datosCliente, string alias = "MARCOS")
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(Guid.NewGuid()).Value;
        
        var result = Customer.Domain.Customer.Crear(id, tenantId, alias, datosCliente.TaxId, datosCliente.RazonSocial, 
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
            
        Add(new CrearClienteTestsHelper.DatosCrearClienteRecord(OtherTaxId.Create("CR-2024-000123", TaxIdType.ResidenceCertificate,
                "FR").Value, null, "Marcos Siderúrgicas S.L.", null, null, null
        ));
    }
    
    public static readonly CrearClienteTestsHelper.DatosCrearClienteRecord ClienteNifNoPersonaJuridica1 = new (
        SpanishTaxId.Crear("47180975V").Value,
        Address.Crear("Calle Ramón y Cajal 34", null, null, null,
            null, null).Value, null, "Marcos Alexis", "García", "Machado"
    );

    public static readonly CrearClienteTestsHelper.DatosCrearClienteRecord ClienteNifPersonaJuridica1 = new (
        SpanishTaxId.Crear("U89145882").Value,
        Address.Crear("Calle Ramón y Cajal 34", null, null, null,
            null, null).Value,
        "Marcos Siderúrgicas S.L.", null, null, null
    );
    
    public static readonly CrearClienteTestsHelper.DatosCrearClienteRecord ClienteIdOtroCertificadoResidencia1 = new (OtherTaxId.Create("CR-2024-000123", TaxIdType.ResidenceCertificate,
            "FR").Value, Address.Crear("Calle Ramón y Cajal 23", "48001", "Bilbao",
            "Bizkaia", "País Vasco", "España").Value, "Marcos Siderúrgicas S.L.", null, null, null
    );
}