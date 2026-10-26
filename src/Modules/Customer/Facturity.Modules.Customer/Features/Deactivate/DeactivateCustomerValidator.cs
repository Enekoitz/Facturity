using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class DeactivateCustomerCommandValidator : AbstractValidator<DeactivateCustomerCommand>
{
    public DeactivateCustomerCommandValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("Customer identifier is required.");
    }
}