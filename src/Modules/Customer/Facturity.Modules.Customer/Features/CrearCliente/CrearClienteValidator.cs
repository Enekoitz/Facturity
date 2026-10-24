using Facturity.Modules.Customer.Domain;
using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal class CrearClienteValidator: AbstractValidator<CrearClienteCommand>
{
    public CrearClienteValidator()
    {
        RuleFor(c => c.Alias)
            .NotEmpty()
            .MaximumLength(Parameters.Customer.AliasMaxLength);
        
        RuleFor(c => c.Nombre)
            .MaximumLength(Parameters.Customer.NameMaxLength);
        
        RuleFor(c => c.PrimerApellido)
            .MaximumLength(Parameters.Customer.SurnameMaxLength);
        
        RuleFor(c => c.SegundoApellido)
            .MaximumLength(Parameters.Customer.SurnameMaxLength);
        
        RuleFor(c => c.CodigoPostal)
            .MaximumLength(Parameters.Address.PostalCodeExactLength);
        
        RuleFor(c => c.Direccion)
            .NotEmpty()
            .MaximumLength(Parameters.Address.AddressMaxLength);
        
        RuleFor(c => c.Municipio)
            .MaximumLength(Parameters.Address.CityMaxLength);
        
        RuleFor(c => c.Provincia)
            .MaximumLength(Parameters.Address.StateMaxLength);
        
        RuleFor(c => c.Region)
            .MaximumLength(Parameters.Address.RegionMaxLength);
        
        RuleFor(c => c.Pais)
            .MaximumLength(Parameters.Address.CountryMaxLength);
        
        RuleFor(c => c.IdentificacionFiscalValor)
            .NotEmpty()
            .MaximumLength(30);
        
        RuleFor(c => c.IdentificacionFiscalTipo)
            .IsInEnum();
        
        RuleFor(c => c.IdentificacionFiscalCodigoPais)
            .Matches(@"^[A-Za-z]{2}$")
            .When(c => c.IdentificacionFiscalCodigoPais != null)
            .WithMessage("The ISO code must be null or contain exactly 2 letters.");
    }
}