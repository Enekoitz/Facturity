using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class DesactivarClienteCommandValidator : AbstractValidator<DesactivarClienteCommand>
{
    public DesactivarClienteCommandValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("El identificador no es válido");
    }
}