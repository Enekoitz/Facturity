using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

internal static class CustomerErrors
{
    internal static readonly Error IdInvalid =
        Error.Validation("Customer.IdInvalid", "Customer identifier is invalid.");

    internal static readonly Error AliasEmpty =
        Error.Validation("Customer.AliasEmpty", "Alias is required.");

    internal static readonly Error AliasTooLong = 
        Error.Validation("Customer.AliasTooLong",
        $"Alias must not exceed {Parameters.CustomerParameters.AliasMaxLength} characters.");

    internal static readonly Error NameEmpty = 
        Error.Validation("Customer.NameEmpty",
        "Name is required.");
    
    internal static readonly Error NameTooLong = 
        Error.Validation("Customer.NameTooLong",
        $"Name must not exceed {Parameters.CustomerParameters.NameMaxLength} characters.");

    internal static readonly Error FirstSurnameEmpty = 
        Error.Validation("Customer.FirstSurnameEmpty", 
        "First surname is required.");
    
    internal static readonly Error FirstSurnameTooLong = 
        Error.Validation("Customer.FirstSurnameTooLong",
        $"First surname must not exceed {Parameters.CustomerParameters.SurnameMaxLength} characters.");
    
    internal static readonly Error SecondSurnameTooLong = 
        Error.Validation("Customer.SecondSurnameTooLong",
        $"Second surname must not exceed {Parameters.CustomerParameters.SurnameMaxLength} characters.");

    internal static readonly Error LegalNameEmpty = 
        Error.Validation("Customer.LegalNameEmpty",
        "Legal name is required.");

    internal static readonly Error LegalNameTooLong = 
        Error.Validation("Customer.LegalNameTooLong",
        $"Legal name must not exceed {Parameters.CustomerParameters.LegalNameMaxLength} characters.");
    
    internal static readonly Error InvalidTaxDesignationType = 
        Error.Validation("Customer.InvalidTaxDesignationType",
        "Tax designation type is invalid.");
    
    internal static readonly Error NotFound =
        Error.NotFound("Customer.NotFound", 
            "Customer not found.");

    internal static readonly Error EmailInUse =
        Error.Conflict("Customer.EmailInUse", 
            "Customer email already exists.");
    
    internal static readonly Error AlreadyDeleted =
        Error.NotFound("Customer.PreviouslyDeleted", 
            "Customer has already been deleted.");
    
    internal static readonly Error InvalidFutureDate = 
        Error.Unexpected("Customer.InvalidFutureDate",
        "Date must not be in the future.");
    
    internal static readonly Error SimultaneousActivationDeactivationDate = 
        Error.Unexpected("Customer.SimultaneousActivationDeactivationDate",
        "It is not possible to provide the activation and deactivation dates simultaneously.");
    
    internal static readonly Error TaxIdTypeInvalid = 
        Error.Validation("Customer.TaxIdTypeInvalid",
        $"Tax ID type is invalid.");
    
    internal static readonly Error AliasInUse =
        Error.Conflict("Customer.AliasInUse", 
            "Alias is already in use.");
    
    internal static readonly Error TaxIdInUse =
        Error.Conflict("Customer.TaxIdInUse", 
            "Tax ID is already in use.");
}