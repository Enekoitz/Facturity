using Facturity.Modules.Customer.Domain;
using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class ObtenerClientePorIdValidator : AbstractValidator<ObtenerClientePorIdQuery>
{
    public ObtenerClientePorIdValidator()
    {
        RuleFor(c => c.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage("El identificador no es válido");
    }
}