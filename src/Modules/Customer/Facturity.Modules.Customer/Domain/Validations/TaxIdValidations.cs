using System.Text.RegularExpressions;

namespace Facturity.Modules.Customer.Domain;

public static partial class TaxIdValidations
{
    //TODO:Translate into english
    private const string SpanishTaxIdLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

    private static readonly HashSet<string> EuCountries = new(StringComparer.OrdinalIgnoreCase)
    {
        "AT", "BE", "BG", "CY", "CZ", "DE", "DK", "EE", "EL", "FI",
        "FR", "HR", "HU", "IE", "IT", "LT", "LU", "LV", "MT", "NL",
        "PL", "PT", "RO", "SE", "SI", "SK"
    };

    public static bool IsValidSpanishTaxId(string? value, out string errorCode, out string errorDescription)
    {
        errorCode = string.Empty;
        errorDescription = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            errorCode = "Customer.TaxId.SpanishTaxIdEmpty";
            errorDescription = "Spanish tax ID (NIF) is required.";
            return false;
        }

        value = value.Trim().ToUpper();

        if (value.Length != 9)
        {
            errorCode = "Customer.TaxId.SpanishTaxIdTooLong";
            errorDescription = $"Spanish tax ID (NIF) must have 9 characters.";
            return false;
        }

        if (IsDni(value))
            return IsValidDni(value, out errorCode, out errorDescription);

        if (IsNie(value))
            return IsValidNie(value, out errorCode, out errorDescription);

        if (IsCif(value))
            return IsValidCif(value, out errorCode, out errorDescription);

        errorCode = "Customer.TaxId.SpanishTaxIdInvalidFormat";
        errorDescription = "Spanish tax ID (NIF) does not have a valid format or check digit.";
        return false;
    }

    public static bool IsValidDni(string value, out string errorCode, out string errorDescription)
    {
        errorCode = string.Empty;
        errorDescription = string.Empty;

        value = value.ToUpper().Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            errorCode = "Customer.TaxId.DniEmpty";
            errorDescription = "DNI is required.";
            return false;
        }

        if (!int.TryParse(value[..8], out _))
        {
            errorCode = "Customer.TaxId.DniFirstCharactersInvalid";
            errorDescription = "The first 8 characters of the DNI are invalid.";
            return false;
        }

        if (value.Trim().Length != 9)
        {
            errorCode = "Customer.TaxId.DniInvalidLength";
            errorDescription = "The DNI must have 9 characters.";
            return false;
        }

        // Standard DNI format: 8 digits + letter
        var regex = MyDniRegex();
        if (!regex.IsMatch(value))
        {
            errorCode = "Customer.TaxId.DniInvalidFormat";
            errorDescription = "The DNI has an invalid format.";
            return false;
        }

        int number = int.Parse(value[..8]);
        char expectedLetter = SpanishTaxIdLetters[number % 23];
        char actualLetter = value[^1];

        if (expectedLetter != actualLetter)
        {
            errorCode = "Customer.TaxId.DniInvalidLetter";
            errorDescription = "The letter in the DNI is invalid.";
            return false;
        }

        return true;
    }

    public static bool IsValidNie(string nie, out string errorCode, out string errorDescription)
    {
        errorCode = string.Empty;
        errorDescription = string.Empty;

        if (string.IsNullOrWhiteSpace(nie))
        {
            errorCode = "Customer.TaxId.NieEmpty";
            errorDescription = "Nie is required.";
            return false;
        }

        nie = nie.ToUpper().Trim();

        if (nie.Length != 9)
        {
            errorCode = "Customer.TaxId.NieInvalidLength";
            errorDescription = "NIE must have 9 characters.";
            return false;
        }

        // Permitted formats for NIE: X/Y/Z + 7 digits + letter
        var regex = MyNieRegex();
        if (!regex.IsMatch(nie))
        {
            errorCode = "Customer.TaxId.NieInvalidFormat";
            errorDescription = "The NIE has an invalid format.";
            return false;
        }

        // Conversion of the initial letter into a number for use in the SpanishTaxId algorithm
        char firstLetter = nie[0];
        string numericalPart = firstLetter switch
        {
            'X' => "0",
            'Y' => "1",
            'Z' => "2",
            _ => string.Empty
        };

        if (numericalPart.Length == 0)
        {
            errorCode = "Customer.TaxId.NieFirstLetterInvalid";
            errorDescription = "The first letter of the NIE is invalid.";
            return false;
        }

        // We complete it with the next 7 digits
        numericalPart += nie.Substring(1, 7);

        if (!int.TryParse(numericalPart, out int number))
        {
            errorCode = "Customer.TaxId.NieInvalidFormat";
            errorDescription = "The NIE has an invalid format.";
            return false;
        }
            
        char expectedLetter = SpanishTaxIdLetters[number % 23];
        char actualLetter = nie[^1];

        if (expectedLetter != actualLetter)
        {
            errorCode = "Customer.TaxId.NieInvalidLastLetter";
            errorDescription = "The last letter of the NIE is invalid.";
            return false;
        }

        return true;
    }

    public static bool IsValidCif(string cif, out string errorCode, out string errorDescription)
    {
        errorCode = string.Empty;
        errorDescription = string.Empty;

        if (string.IsNullOrWhiteSpace(cif))
        {
            errorCode = "Customer.TaxId.CifEmpty";
            errorDescription = "CIF is required.";
            return false;
        }

        cif = cif.Trim().ToUpper();

        // It must be exactly 9 characters long
        if (cif.Length != 9)
        {
            errorCode = "Customer.TaxId.CifInvalidLength";
            errorDescription = "CIF must have 9 characters.";
            return false;
        }

        char letter = cif[0];
        string numbers = cif.Substring(1, 7);
        char control = cif[8];

        // Valid initial letter
        if (!"ABCDEFGHJKLMNPQRSUVW".Contains(letter))
        {
            errorCode = "Customer.TaxId.CifInvalidFirstLetter";
            errorDescription = "The first letter of the CIF is invalid.";
            return false;
        }

        // The next 7 must be digits
        if (!numbers.All(char.IsDigit))
        {
            errorCode = "Customer.TaxId.CifInvalidFormat";
            errorDescription = "The CIF has an invalid format.";
            return false;
        }

        // Calculation of the check digit
        int sumEvenNumbers = 0;
        int sumOddNumbers = 0;

        for (int i = 0; i < numbers.Length; i++)
        {
            int n = numbers[i] - '0';

            if (i % 2 == 0)
            {
                int doubleValue = n * 2;
                if (doubleValue >= 10)
                    doubleValue = (doubleValue / 10) + (doubleValue % 10);

                sumOddNumbers += doubleValue;
            }
            else
            {
                sumEvenNumbers += n;
            }
        }

        int totalSum = sumEvenNumbers + sumOddNumbers;
        int calculatedDigit = 10 - (totalSum % 10);
        if (calculatedDigit == 10)
            calculatedDigit = 0;

        char checkDigitNumber = (char)('0' + calculatedDigit);
        char checkDigitLetter = "JABCDEFGHI"[calculatedDigit];

        bool controlMustBeALetter = "PQSW".Contains(letter);
        bool controlCanBeLetterOrNumber = "ABEH".Contains(letter);
        bool controlMustBeANumber = !controlMustBeALetter && !controlCanBeLetterOrNumber;

        bool isValid = controlMustBeALetter 
            ? control == checkDigitLetter
            : controlCanBeLetterOrNumber 
                ? (control == checkDigitLetter || control == checkDigitNumber)
                : control == checkDigitNumber;

        if (!isValid)
        {
            errorCode = "Customer.TaxId.CifInvalidFormat";
            errorDescription = "The CIF has an invalid format.";
            return false;
        }
                
        return true;
    }

    public static bool IsValidOtherTaxId(
        string countryCode, 
        TaxIdType type, 
        string id, 
        out string errorCode, 
        out string errorDescription)
    {
        errorCode = string.Empty;
        errorDescription = string.Empty;

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            errorCode = "Customer.TaxId.OtherTaxIdCountryCodeEmpty";
            errorDescription = "Country code is required.";
        }
        else if (string.IsNullOrWhiteSpace(id))
        {
            errorCode = "Customer.TaxId.OtherTaxIdEmpty";
            errorDescription = "Tax ID is required.";
        }
        else if (type == TaxIdType.TaxId)
        {
            errorCode = "Customer.TaxId.OtherTaxIdTypeInvalid";
            errorDescription = "Other tax ID type cannot be spanish tax ID.";
        }
        else
        {
            countryCode = countryCode.Trim().ToUpper();
            id = id.Trim();

            // ISO-2 format
            if (countryCode.Length != 2)
            {
                errorCode = "Customer.TaxId.CountryCodeLengthNoIso2";
                errorDescription = "Ccountry code must be exactly 2 ISO-2 characters.";
            }
            // Using 'ES' in OtherTaxId is prohibited.
            else if (countryCode == "ES")
            {
                errorCode = "Customer.TaxId.OtherTaxIdSpanishCountryCode";
                errorDescription = "For Spanish tax identification, SpanishTaxId must be used, not OtherId.";
            }
            // Standard maximum length for the TicketBAI XSD (1 to 20 characters)
            else if (id.Length is < 1 or > 20)
            {
                errorCode = "Customer.TaxId.OtherTaxIdInvalidLength";
                errorDescription = "Other tax ID must be between 1 and 20 characters long.";
            }
            // Specific rule for intra-Community VAT ID (Type 02)
            else if (type == TaxIdType.IntraCommunityVatId && !IsEuCountry(countryCode))
            {
                errorCode = "Customer.TaxId.OtherTaxIdIntraCommunityVatIdInvalid";
                errorDescription = $"NIF-IVA (02) type is only valid for EU member countries. '{countryCode}' does not belong to the EU.";
            }
            // Removing prohibited characters from XML
            else if (ContainsInvalidXmlCharacters(id))
            {
                errorCode = "Customer.TaxId.OtherTaxIdInvalidCharacters";
                errorDescription = "Identifier contains characters that are invalid for an XML signature.";
            }
        }
        
        return errorCode.Length == 0;
    }

    public static bool IsEuCountry(string countryCode)
        => !string.IsNullOrWhiteSpace(countryCode) && EuCountries.Contains(countryCode.Trim());

    private static bool ContainsInvalidXmlCharacters(string text)
    {
        // Checks whether it contains characters outside the standard printable ASCII range
        return text.Any(ch => ch < 32 || ch > 126);
    }

    /// <summary>
    /// Validates a DNI (SpanishTaxId for a Spanish individual). 
    /// Format: 8 digits + a check letter. 
    /// </summary>
    public static bool IsDni(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;

        value = value.Trim().ToUpper();
        var regex = MyDniRegex();
        if (!regex.IsMatch(value)) return false;

        int number = int.Parse(value[..8]);
        char expectedLetter = SpanishTaxIdLetters[number % 23];
        return value[^1] == expectedLetter;
    }

    /// <summary>
    /// Validates a NIE (SpanishTaxId for foreigners residing in Spain). 
    /// Format: X/Y/Z + 7 digits + control letter. 
    /// </summary>
    public static bool IsNie(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;

        value = value.Trim().ToUpper();
        var regex = MyNieRegex();
        if (!regex.IsMatch(value)) return false;

        // We convert the initial letter into a number: X=0, Y=1, Z=2
        int firstPart = value[0] switch
        {
            'X' => 0,
            'Y' => 1,
            'Z' => 2,
            _ => -1
        };

        if (firstPart == -1) return false;

        string fullNumber = string.Concat(firstPart.ToString(), value.AsSpan(1, 7));
        int number = int.Parse(fullNumber);

        char expectedLetter = SpanishTaxIdLetters[number % 23];
        return value[^1] == expectedLetter;
    }

    /// <summary>
    /// Validates a CIF (tax identification number for a Spanish legal entity). 
    /// Format: an initial letter (A-H, J-N, P-S, U-V) + 7 digits + a check digit. 
    /// </summary>
    public static bool IsCif(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;

        value = value.Trim().ToUpper();
        var regex = MyCifRegex();
        return regex.IsMatch(value);
    }

    [GeneratedRegex(@"^[ABCDEFGHJNPQRSUVW]\d{7}[0-9A-J]$")]
    private static partial Regex MyCifRegex();
    [GeneratedRegex(@"^[XYZ]\d{7}[A-Z]$")]
    private static partial Regex MyNieRegex();
    [GeneratedRegex(@"^\d{8}[A-Z]$")]
    private static partial Regex MyDniRegex();
}

