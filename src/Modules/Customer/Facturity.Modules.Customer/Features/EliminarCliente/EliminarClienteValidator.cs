using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class EliminarClienteCommandValidator : AbstractValidator<EliminarClienteCommand>
{
    public EliminarClienteCommandValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("El identificador no es válido");
    }
}