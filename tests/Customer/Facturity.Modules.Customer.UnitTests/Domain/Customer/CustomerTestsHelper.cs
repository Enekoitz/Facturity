using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public static class UpdateCustomerTestsHelper
{
    public sealed record DataRecord(
        string Alias,
        ITaxId  TaxId,
        string? LegalEntityName,
        string? Name,
        string? FirstSurname,
        string? SecondSurname,
        Address? Address
    ){}
}

public class UpdateCustomerTestsObjects 
    : TheoryData<UpdateCustomerTestsHelper.DataRecord, UpdateCustomerTestsHelper.DataRecord>
{
    public UpdateCustomerTestsObjects()
    {
        Add(Customer1, Customer2);
        Add(Customer2, Customer3);
        Add(Customer3, Customer1);
    }

    public static readonly UpdateCustomerTestsHelper.DataRecord Customer1 = 
        new(
            "MARC",
            SpanishTaxId.Create("47180975V").Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            Address.Create("Calle Ramón y Cajal 34", null, null, null, null, null).Value
        );

    public static readonly UpdateCustomerTestsHelper.DataRecord Customer2 = 
        new(
            "FTNPARK",
            SpanishTaxId.Create("A31497027").Value, 
            "FitnessPark", 
            null, 
            null, 
            null,
            Address.Create("Polígono Industrial Lapatza, Aldatza Kalea, 2", "48970", "Basauri", "Bizkaia", "Euskadi", "España").Value
        );

    public static readonly UpdateCustomerTestsHelper.DataRecord Customer3 = 
        new(
            "FRANC",
            OtherTaxId.Create("CR-2024-000123", 
                TaxIdType.ResidenceCertificate,
                "FR").Value, 
            "Francais S.A.", 
            null, 
            null, 
            null,
            Address.Create("33, Chemin de Saugane", "12290", "Prades-Salars", "Aveyron", "Occitanie", "France").Value
        );
}

public static class RebuildCustomerTestsHelper
{
    public sealed record Data(
        ITaxId  TaxId,
        Address? Address,
        string? LegalEntityName,
        string? Name,
        string? FirstSurname,
        string? SecondSurname,
        DateTimeOffset? ActivatedAtUtc,
        DateTimeOffset? DeactivatedAtUtc,
        DateTimeOffset? DeletedAtUtc
        ){}

    public static readonly Data DataObject = new(
        SpanishTaxId.Create("47180975V").Value, 
        Address.Create("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
        null, 
        "Marcos Alexis", 
        "García", 
        "Machado",
        null,
        new DateTimeOffset(2026, 08, 23, 08, 05, 45, TimeSpan.Zero),
        null
    );
}

public class RebuildCustomerTestsObjects : TheoryData<RebuildCustomerTestsHelper.Data>
{
    public RebuildCustomerTestsObjects()
    {
        Add(CustomerWithActivatedAtUtc);
        Add(CustomerWithDeactivatedAtUtc);
        Add(CustomerWithDeletedAtUtc);
    }
    
    public static readonly RebuildCustomerTestsHelper.Data CustomerWithActivatedAtUtc = 
        new(
            SpanishTaxId.Create("47180975V").Value, 
            Address.Create("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            new DateTimeOffset(2026, 08, 23, 08, 05, 45, TimeSpan.Zero),
            null,
            null
        );

    public static readonly RebuildCustomerTestsHelper.Data CustomerWithDeactivatedAtUtc = 
        new(
            SpanishTaxId.Create("47180975V").Value, 
            Address.Create("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            null,
            new DateTimeOffset(2026, 06, 12, 08, 43, 12, TimeSpan.Zero),
            null
        );
    
    public static readonly RebuildCustomerTestsHelper.Data CustomerWithDeletedAtUtc = 
        new(
            SpanishTaxId.Create("47180975V").Value, 
            Address.Create("Calle Ramón y Cajal 34", null, null, null, null, null).Value, 
            null, 
            "Marcos Alexis", 
            "García", 
            "Machado",
            null,
            null,
            new DateTimeOffset(2026, 07, 23, 21, 53, 00, TimeSpan.Zero)
        );
}

public static class CreateCustomerTestsHelper
{
    public sealed record Data(
        ITaxId  TaxId,
        Address? Address,
        string? LegalEntityName,
        string? Name,
        string? FirstSurname,
        string? SecondSurname){}
    
    public static Result<Customer.Domain.Customer> CreateCustomer(Data datosCliente, in Guid id , in TenantId tenantId, in string alias)
    {
        var result = Customer.Domain.Customer.Create(id, tenantId, alias, datosCliente.TaxId, datosCliente.LegalEntityName, 
            datosCliente.Name, datosCliente.FirstSurname, datosCliente.SecondSurname, datosCliente.Address);

        return result;
    }
    
    public static Result<Customer.Domain.Customer> CreateCustomer(Data datosCliente, string alias = "MARCOS")
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Create(Guid.NewGuid()).Value;
        
        var result = Customer.Domain.Customer.Create(id, tenantId, alias, datosCliente.TaxId, datosCliente.LegalEntityName, 
            datosCliente.Name, datosCliente.FirstSurname, datosCliente.SecondSurname, datosCliente.Address);

        return result;
    }
}

public class CreateCustomerTestsObjects : TheoryData<CreateCustomerTestsHelper.Data>
{
    public CreateCustomerTestsObjects()
    {
        Add(CustomerNotIsLegalEntity);
        Add(CustomerIsLegalEntity);
        Add(CustomerOtherTaxId);
            
        Add(new CreateCustomerTestsHelper.Data(OtherTaxId.Create("CR-2024-000123", TaxIdType.ResidenceCertificate,
                "FR").Value, null, "Marcos Siderúrgicas S.L.", null, null, null
        ));
    }
    
    public static readonly CreateCustomerTestsHelper.Data CustomerNotIsLegalEntity = new (
        SpanishTaxId.Create("47180975V").Value,
        Address.Create("Calle Ramón y Cajal 34", null, null, null,
            null, null).Value, null, "Marcos Alexis", "García", "Machado"
    );

    public static readonly CreateCustomerTestsHelper.Data CustomerIsLegalEntity = new (
        SpanishTaxId.Create("U89145882").Value,
        Address.Create("Calle Ramón y Cajal 34", null, null, null,
            null, null).Value,
        "Marcos Siderúrgicas S.L.", null, null, null
    );
    
    public static readonly CreateCustomerTestsHelper.Data CustomerOtherTaxId = new (OtherTaxId.Create("CR-2024-000123", TaxIdType.ResidenceCertificate,
            "FR").Value, Address.Create("Calle Ramón y Cajal 23", "48001", "Bilbao",
            "Bizkaia", "País Vasco", "España").Value, "Marcos Siderúrgicas S.L.", null, null, null
    );
}