using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal sealed class ActivateCustomerCommandValidator : AbstractValidator<ActivateCustomerCommand>
{
    internal ActivateCustomerCommandValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("Customer identifier is required.");
    }
}