using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class ActivateCustomerCommandValidator : AbstractValidator<ActivateCustomerCommand>
{
    public ActivateCustomerCommandValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("The identifier is required.");
    }
}