using Facturity.Modules.Customer.Domain;
using FluentValidation;

namespace Facturity.Modules.Customer.Features;

internal sealed class CreateCustomerValidator: AbstractValidator<CreateCustomerCommand>
{
    internal CreateCustomerValidator()
    {
        RuleFor(c => c.Alias)
            .NotEmpty()
            .MaximumLength(Parameters.Customer.AliasMaxLength);
        
        RuleFor(c => c.Name)
            .MaximumLength(Parameters.Customer.NameMaxLength);
        
        RuleFor(c => c.FirstSurname)
            .MaximumLength(Parameters.Customer.SurnameMaxLength);
        
        RuleFor(c => c.SecondSurname)
            .MaximumLength(Parameters.Customer.SurnameMaxLength);
        
        RuleFor(c => c.PostalCode)
            .MaximumLength(Parameters.Address.PostalCodeExactLength);
        
        RuleFor(c => c.Address)
            .NotEmpty()
            .MaximumLength(Parameters.Address.AddressMaxLength);
        
        RuleFor(c => c.City)
            .MaximumLength(Parameters.Address.CityMaxLength);
        
        RuleFor(c => c.State)
            .MaximumLength(Parameters.Address.StateMaxLength);
        
        RuleFor(c => c.Region)
            .MaximumLength(Parameters.Address.RegionMaxLength);
        
        RuleFor(c => c.Country)
            .MaximumLength(Parameters.Address.CountryMaxLength);
        
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