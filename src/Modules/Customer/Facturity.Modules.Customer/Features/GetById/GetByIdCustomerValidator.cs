using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class GetByIdCustomerValidator : AbstractValidator<GetByIdCustomerQuery>
{
    public GetByIdCustomerValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("Customer identifier is required.");
    }
}