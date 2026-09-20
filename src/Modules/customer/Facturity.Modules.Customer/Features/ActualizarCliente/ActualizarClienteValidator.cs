using Facturity.Modules.Customer.Domain;
using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class ActualizarClienteValidator: AbstractValidator<ActualizarClienteCommand>
{
    public ActualizarClienteValidator()
    {
        RuleFor(c => c.Nombre)
            .MaximumLength(ClienteParametros.NombreMaxLength);
        
        RuleFor(c => c.PrimerApellido)
            .MaximumLength(ClienteParametros.ApellidoMaxLength);
        
        RuleFor(c => c.SegundoApellido)
            .MaximumLength(ClienteParametros.ApellidoMaxLength);
        
        RuleFor(c => c.CodigoPostal)
            .MaximumLength(DomicilioParametros.CodigoPostalLength);
        
        RuleFor(c => c.Direccion)
            .NotEmpty()
            .MaximumLength(DomicilioParametros.DireccionMaxLength);
        
        RuleFor(c => c.Municipio)
            .MaximumLength(DomicilioParametros.MunicipioMaxLength);
        
        RuleFor(c => c.Provincia)
            .MaximumLength(DomicilioParametros.ProvinciaMaxLength);
        
        RuleFor(c => c.Region)
            .MaximumLength(DomicilioParametros.RegionMaxLength);
        
        RuleFor(c => c.Pais)
            .MaximumLength(DomicilioParametros.PaisMaxLength);
        
        RuleFor(c => c.IdentificacionFiscalValor)
            .NotEmpty()
            .MaximumLength(30);
        
        RuleFor(c => c.IdentificacionFiscalTipo)
            .IsInEnum();
        
        RuleFor(c => c.IdentificacionFiscalCodigoPais)
            .Matches(@"^[A-Za-z]{2}$")
            .When(c => c.IdentificacionFiscalCodigoPais != null)
            .WithMessage("El código ISO debe ser nulo o contener exactamente 2 letras.");
    }
}