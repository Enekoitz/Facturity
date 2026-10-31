using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed class InvoiceSeries : Entity<Guid>, ITenantEntity
{
    internal const int NameMaxLength = 20;
    internal const int DescriptionMaxLength = 250;
    
    public TenantId TenantId{get; private set; }
    internal string Name { get; private init; }
    internal string? Description { get; private init; } 

    private InvoiceSeries(Guid id, TenantId tenantId, string name, string? description)
        : base(id)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
    }
    
    private InvoiceSeries(){ }

    internal static Result<InvoiceSeries> Create(Guid id, TenantId tenantId,string name, string? description)
    { 
        List<Error> errors = [];
        
        if (id == Guid.Empty)
            errors.Add(InvoiceSeriesErrors.IdInvalid);

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(InvoiceSeriesErrors.NameEmpty);
        }
        else
        {
            name = name?.Trim()?.ToUpper();
            
            if(name?.Length > NameMaxLength)
                errors.Add(InvoiceSeriesErrors.NameTooLong);
        }
        
        description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        if( description?.Length > DescriptionMaxLength)
            errors.Add(InvoiceSeriesErrors.DescriptionTooLong);

        return errors.Count > 0
            ? Result.Fail<InvoiceSeries>(errors)
            : Result.Ok(new InvoiceSeries(id, tenantId, name, description));
    }
}