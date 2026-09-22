using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class ActivarClienteCommandValidator : AbstractValidator<ActivarClienteCommand>
{
    public ActivarClienteCommandValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("El identificador no es válido");
    }
}