using Facturity.Modules.Customer.Domain;
using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class ObtenerClientePorIdValidator : AbstractValidator<ObtenerClientePorIdQuery>
{
    public ObtenerClientePorIdValidator()
    {
        // Datos del Cliente
        RuleFor(c => c.Id)
            .NotEmpty()
            .MaximumLength(ClienteParametros.AliasMaxLength);
    }
}