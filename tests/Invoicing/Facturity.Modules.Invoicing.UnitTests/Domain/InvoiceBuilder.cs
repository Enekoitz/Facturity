using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public static class InvoiceBuilder
{
    internal static Invoice BuildDraft(bool isSimplified, Guid? id = null, TenantId? tenantId = null)
    {
        id ??= Guid.CreateVersion7();
        tenantId ??= Build.Tenant();
        
        var result = Invoice.CreateDraft(id.Value, tenantId.Value, isSimplified);
        
        if (!result.IsSuccess)
            throw new InvalidOperationException($"The draft could not be created during setup: {result.Errors}");

        return result.Value;
    }

    extension(Invoice invoice)
    {
        internal Invoice WithCustomer(Guid customerId)
        {
            var result = invoice.ChangeCustomer(customerId);
            if (!result.IsSuccess)
                throw new InvalidOperationException($"Error setting the customer: {result.Errors}");
            
            return invoice;
        }

        internal Invoice WithoutCustomer()
        {
            invoice.ChangeCustomer(null);
            return invoice;
        }
        
        internal Invoice WithLine(InvoiceLine line)
        {
            var result = invoice.AddLine(line);
            if (!result.IsSuccess)
                throw new InvalidOperationException($"Error setting the customer: {result.Errors}");
            
            return invoice;
        }
        
        internal Invoice WithLine()
        {
            var line = Build.GetValidInvoiceLine(invoice.TenantId, invoice.Id);
            var result = invoice.AddLine(line);
            if (!result.IsSuccess)
                throw new InvalidOperationException($"Error setting the customer: {result.Errors}");
            
            return invoice;
        }
    }
}