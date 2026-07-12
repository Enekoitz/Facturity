using System;
using System.Collections.Generic;
using System.Text;

namespace UgeFactura.Modules.DomainSharedKernel
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OcurridoEn { get; }
    }
}
