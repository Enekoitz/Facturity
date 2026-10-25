using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("Customer identifier is required.");
    }
}