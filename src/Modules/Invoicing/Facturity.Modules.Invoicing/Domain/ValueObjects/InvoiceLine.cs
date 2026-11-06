using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed class InvoiceLine : Entity<Guid>, ITenantEntity
{
    internal const int DescriptionMaxLength = 100;
    internal const int UnitsMaxDecimalPlaces = 2;
    
    
    public TenantId TenantId { get; private init; }
    internal Guid InvoiceId { get; private set; }
    internal string Description { get; private set; }  
    internal decimal Units { get; private set; }
    internal UnitPrice UnitPrice { get; private set; }
    internal Money TaxBase{ get; private set; }
    internal TaxRate TaxRate { get; private set; }
    internal Money TaxAmount { get; private set; }
    internal Money TotalAmount { get; private set; }

    private InvoiceLine(Guid id, TenantId tenantId, Guid invoiceId, string description, decimal units, UnitPrice unitPrice, 
                        Money taxBase, TaxRate taxRate, Money taxAmount, Money totalAmount) : base(id)
    {
        TenantId = tenantId;
        InvoiceId = invoiceId;
        Description = description;
        Units = units;
        UnitPrice = unitPrice;
        TaxBase = taxBase;
        TaxRate = taxRate;
        TaxAmount = taxAmount;
        TotalAmount = totalAmount;
    }
    
    private InvoiceLine(){ }

    internal static Result<InvoiceLine> Create(Guid id, TenantId tenantId, Guid invoiceId, 
                                                string description, decimal units, 
                                                UnitPrice unitPrice, TaxRate taxRate)
    {
        List<Error> errors = [];
        
        if(id == Guid.Empty)
            errors.Add(InvoiceLineError.InvoiceLineIdInvalid);
        
        if(invoiceId == Guid.Empty)
            errors.Add(InvoiceLineError.InvoiceIdInvalid);
        
        var trimmedDescription = description?.Trim();

        if (string.IsNullOrEmpty(trimmedDescription))
            errors.Add(InvoiceLineError.DescriptionEmpty);
        else if (trimmedDescription.Length > DescriptionMaxLength)
            errors.Add(InvoiceLineError.DescriptionTooLong);
        
        if (units <= 0)
            errors.Add(InvoiceLineError.UnitsNegativeOrZero);
        
        if (Utilities.GetSignificantDecimalPlaces(units) > UnitsMaxDecimalPlaces)
            errors.Add(InvoiceLineError.UnitsTooMuchDecimalPlaces);
        
        if (errors.Count > 0)
            return Result.Fail<InvoiceLine>(errors);
     
        var taxBaseResult = Money.Create(TaxEngine.CalculateTaxBase(units, unitPrice));
        if (taxBaseResult.IsFailure)
            return Result.Fail<InvoiceLine>(taxBaseResult.Errors);
        
        var taxAmountResult = Money.Create(TaxEngine.CalculateTaxAmount(taxBaseResult.Value, taxRate));
        if (taxAmountResult.IsFailure)
            return Result.Fail<InvoiceLine>(taxAmountResult.Errors);
        
        var totalAmountResult = Money.Create(taxAmountResult.Value.Amount +  taxBaseResult.Value.Amount);
        if (totalAmountResult.IsFailure)
            return Result.Fail<InvoiceLine>(totalAmountResult.Errors);
        
        return Result.Ok(new InvoiceLine(id, tenantId, invoiceId, trimmedDescription!, units, unitPrice, taxBaseResult.Value, 
                                        taxRate, taxAmountResult.Value, totalAmountResult.Value));
    }
}