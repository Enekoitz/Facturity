using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed class InvoiceSeries : Entity<Guid>, ITenantEntity
{
    internal const int NameMaxLength = 20;
    internal const int DescriptionMaxLength = 250;
    
    public TenantId TenantId{get; private set; }
    internal string Name { get; private init; }
    internal string? Description { get; private init; } 

    private InvoiceSeries(string name, string? description)
    {
        Name = name;
        Description = description;
    }
    
    private InvoiceSeries(){ }

    internal static Result<InvoiceSeries> Create(string name, string? description)
    { 
        List<Error> errors = [];

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
            : Result.Ok(new InvoiceSeries(name, description));
    }
}