using Facturity.Modules.Customer.Domain;
using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal sealed class CreateCustomerValidator: AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(c => c.Alias)
            .NotEmpty()
            .MaximumLength(Parameters.CustomerParameters.AliasMaxLength);
        
        RuleFor(c => c.Name)
            .MaximumLength(Parameters.CustomerParameters.NameMaxLength);
        
        RuleFor(c => c.FirstSurname)
            .MaximumLength(Parameters.CustomerParameters.SurnameMaxLength);
        
        RuleFor(c => c.SecondSurname)
            .MaximumLength(Parameters.CustomerParameters.SurnameMaxLength);
        
        RuleFor(c => c.PostalCode)
            .MaximumLength(Parameters.AddressParameters.PostalCodeExactLength);
        
        RuleFor(c => c.Address)
            .NotEmpty()
            .MaximumLength(Parameters.AddressParameters.AddressMaxLength);
        
        RuleFor(c => c.City)
            .MaximumLength(Parameters.AddressParameters.CityMaxLength);
        
        RuleFor(c => c.State)
            .MaximumLength(Parameters.AddressParameters.StateMaxLength);
        
        RuleFor(c => c.Region)
            .MaximumLength(Parameters.AddressParameters.RegionMaxLength);
        
        RuleFor(c => c.Country)
            .MaximumLength(Parameters.AddressParameters.CountryMaxLength);
        
        RuleFor(c => c.TaxIdValue)
            .NotEmpty()
            .MaximumLength(30);
        
        RuleFor(c => c.TaxIdType)
            .IsInEnum();
        
        RuleFor(c => c.TaxIdCountryCode)
            .Matches(@"^[A-Za-z]{2}$")
            .When(c => c.TaxIdCountryCode != null)
            .WithMessage("The ISO code must be null or contain exactly 2 letters.");
    }
}