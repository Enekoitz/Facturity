using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed class InvoiceSeries : Entity
{
    internal const int NameMaxLength = 20;
    internal string Name { get; private init; }

    private InvoiceSeries(string name)
    {
        Name = name;
    }
    
    private InvoiceSeries(){ }

    internal static Result<InvoiceSeries> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Result.Fail<InvoiceSeries>(InvoiceSeriesErrors.NullOrEmptyName);
        
        name = name.Trim().ToUpper();
        if(name.Length > NameMaxLength)
            return Result.Fail<InvoiceSeries>(InvoiceSeriesErrors.ExceededMaximumLengthName);
        
        return Result.Ok(new InvoiceSeries(name));
    }
}