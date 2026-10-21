using Microsoft.EntityFrameworkCore;
using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.Invoicing.Features;
using Facturity.Modules.SharedKernel.Abstractions;

namespace Facturity.Modules.Invoicing.Infrastructure;

internal  class InvoicingRepository(InvoicingDbContext context) 
    : IInvoicingRepository
{

}
