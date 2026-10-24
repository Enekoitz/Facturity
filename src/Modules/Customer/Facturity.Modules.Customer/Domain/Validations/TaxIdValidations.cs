using System.Text.RegularExpressions;

namespace Facturity.Modules.Customer.Domain;

public static partial class TaxIdValidations
{
    //TODO:Translate into english
    private const string SpanishTaxIdLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

    private static readonly HashSet<string> UeCountries = new(StringComparer.OrdinalIgnoreCase)
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
            errorCode = "Cliente.IdentificacionFiscal.NifVacio";
            errorDescription = "El Nif está vacío.";
            return false;
        }

        value = value.Trim().ToUpper();

        if (value.Length != 9)
        {
            errorCode = "Cliente.IdentificacionFiscal.NifLongitudInvalida";
            errorDescription = "El Nif debe tener 9 caracteres.";
            return false;
        }

        if (IsDni(value))
            return IsValidDni(value, out errorCode, out errorDescription);

        if (IsNie(value))
            return IsValidNie(value, out errorCode, out errorDescription);

        if (IsCif(value))
            return IsValidCif(value, out errorCode, out errorDescription);

        errorCode = "Cliente.IdentificacionFiscal.NifFormatoInvalido";
        errorDescription = "El Nif no tiene un formato o dígito de control válido";
        return false;
    }

    public static bool IsValidDni(string nif, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        nif = nif.ToUpper().Trim();

        if (string.IsNullOrWhiteSpace(nif))
        {
            codigoError = "Cliente.IdentificacionFiscal.DniVacio";
            descripcionError = "El Dni es vacío.";
            return false;
        }

        if (!int.TryParse(nif[..8], out _))
        {
            codigoError = "Cliente.IdentificacionFiscal.DniPrimerosCaracteresInvalidos";
            descripcionError = "Los 8 primeros caracteres del Dni son inválidos.";
            return false;
        }

        if (nif.Trim().Length != 9)
        {
            codigoError = "Cliente.IdentificacionFiscal.DniLongitudInvalida";
            descripcionError = "El Dni debe tener 9 caracteres.";
            return false;
        }

        // Formato Dni estándar: 8 números + letra
        var regex = MyDniRegex();
        if (!regex.IsMatch(nif))
        {
            codigoError = "Cliente.IdentificacionFiscal.DniFormatoInvalido";
            descripcionError = "El Dni tiene un formato inválido.";
            return false;
        }

        int number = int.Parse(nif[..8]);
        char expectedLetter = SpanishTaxIdLetters[number % 23];
        char actualLetter = nif[^1];

        if (expectedLetter != actualLetter)
        {
            codigoError = "Cliente.IdentificacionFiscal.DniLetraIncorrecta";
            descripcionError = "La letra del Dni es inválida.";
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool IsValidNie(string nie, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(nie))
        {
            codigoError = "Cliente.IdentificacionFiscal.NieVacio";
            descripcionError = "El Nie es vacío.";
            return false;
        }

        nie = nie.ToUpper().Trim();

        if (nie.Length != 9)
        {
            codigoError = "Cliente.IdentificacionFiscal.NieLongitudInvalida";
            descripcionError = "El Nie debe tener 9 caracteres.";
            return false;
        }

        // Formatos permitidos Nie: X/Y/Z + 7 dígitos + letra
        var regex = MyNieRegex();
        if (!regex.IsMatch(nie))
        {
            codigoError = "Cliente.IdentificacionFiscal.NieFormatoInvalido";
            descripcionError = "El Nie tiene un formato inválido.";
            return false;
        }

        // Conversión de la letra inicial a número para usar el algoritmo del Nif
        char firstLetter = nie[0];
        string numericPart = firstLetter switch
        {
            'X' => "0",
            'Y' => "1",
            'Z' => "2",
            _ => string.Empty
        };

        if (numericPart.Length == 0)
        {
            codigoError = "Cliente.IdentificacionFiscal.NiePrimeraLetraInvalida";
            descripcionError = "La primera letra del Nie es inválida.";
            return false;
        }

        // Completamos con los 7 dígitos siguientes
        numericPart += nie.Substring(1, 7);

        if (!int.TryParse(numericPart, out int number))
        {
            codigoError = "Cliente.IdentificacionFiscal.NieFormatoInvalido";
            descripcionError = "El Nie tiene un formato inválido.";
            return false;
        }
            
        char expectedLetter = SpanishTaxIdLetters[number % 23];
        char actualLetter = nie[^1];

        if (expectedLetter != actualLetter)
        {
            codigoError = "Cliente.IdentificacionFiscal.NieUltimaLetraInvalida";
            descripcionError = "La última letra del Nie es inválida.";
            return false;
        }

        return true;
    }

    public static bool IsValidCif(string cif, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(cif))
        {
            codigoError = "Cliente.IdentificacionFiscal.CifVacio";
            descripcionError = "El Cif (o Nif de persona jurídica) es vacío.";
            return false;
        }

        cif = cif.Trim().ToUpper();

        // Debe tener 9 caracteres exactos
        if (cif.Length != 9)
        {
            codigoError = "Cliente.IdentificacionFiscal.CifLongitudInvalida";
            descripcionError = "El Cif (o Nif de persona jurídica) debe tener 9 caracteres.";
            return false;
        }

        char letra = cif[0];
        string numeros = cif.Substring(1, 7);
        char control = cif[8];

        // Letra inicial válida
        if (!"ABCDEFGHJKLMNPQRSUVW".Contains(letra))
        {
            codigoError = "Cliente.IdentificacionFiscal.CifPrimeraLetraInvalida";
            descripcionError = "La primera letra del Cif (o Nif de persona jurídica) es inválida.";
            return false;
        }

        // Los 7 siguientes deben ser dígitos
        if (!numeros.All(char.IsDigit))
        {
            codigoError = "Cliente.IdentificacionFiscal.CifFormatoInvalido";
            descripcionError = "El Cif (o Nif de persona jurídica) tiene un formato inválido.";
            return false;
        }

        // Cálculo del dígito de control
        int sumaPares = 0;
        int sumaImpares = 0;

        for (int i = 0; i < numeros.Length; i++)
        {
            int n = numeros[i] - '0';

            // Posiciones impares: 1,3,5,7 → índices 0,2,4,6
            if (i % 2 == 0)
            {
                int doble = n * 2;
                if (doble >= 10)
                    doble = (doble / 10) + (doble % 10); // suma de dígitos

                sumaImpares += doble;
            }
            else // pares
            {
                sumaPares += n;
            }
        }

        int sumaTotal = sumaPares + sumaImpares;
        int digitoCalculado = 10 - (sumaTotal % 10);
        if (digitoCalculado == 10)
            digitoCalculado = 0;

        char digitoControlNumero = (char)('0' + digitoCalculado);
        char digitoControlLetra = "JABCDEFGHI"[digitoCalculado]; // Equivalencia letras

        // Reglas según la letra inicial
        bool controlDebeSerLetra = "PQSW".Contains(letra);
        bool controlPuedeSerLetraONumero = "ABEH".Contains(letra);
        bool controlDebeSerNumero = !controlDebeSerLetra && !controlPuedeSerLetraONumero;

        bool esValido = controlDebeSerLetra 
            ? control == digitoControlLetra
            : controlPuedeSerLetraONumero 
                ? (control == digitoControlLetra || control == digitoControlNumero)
                : control == digitoControlNumero;

        if (!esValido)
        {
            codigoError = "Cliente.IdentificacionFiscal.CifFormatoInvalido";
            descripcionError = "El Cif (o Nif de persona jurídica) tiene un formato inválido.";
            return false;
        }
                
        return true;
    }

    public static bool IsValidOtherTaxId(
        string codigoPais, 
        TaxIdType tipo, 
        string id, 
        out string codigoError, 
        out string descripcionError)
    {

        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(codigoPais))
        {
            codigoError = "Cliente.IdentificacionFiscal.IdOtroExtranjeroCodigoPaisVacio";
            descripcionError = "El código de país es obligatorio.";
        }
        else if (string.IsNullOrWhiteSpace(id))
        {
            codigoError = "Cliente.IdentificacionFiscal.IdOtroExtranjeroIdVacio";
            descripcionError = "El identificador extranjero no puede estar vacío.";
        }
        else if (tipo == TaxIdType.TaxId)
        {
            codigoError = "Cliente.IdentificacionFiscal.IdOtroExtranjeroInvalido";
            descripcionError = "El identificador extranjero no puede ser de tipo NIF español.";
        }
        else
        {
            codigoPais = codigoPais.Trim().ToUpper();
            id = id.Trim();

            // 1. Formato ISO-2
            if (codigoPais.Length != 2)
            {
                codigoError = "Cliente.IdentificacionFiscal.IdOtroExtranjeroCodigoPaisLongitudNoIso2";
                descripcionError = "El código de país debe tener exactamente 2 caracteres ISO-2.";
            }
            // 2. Prohibido utilizar 'ES' en IDOtro
            else if (codigoPais == "ES")
            {
                codigoError = "Cliente.IdentificacionFiscal.IdOtroExtranjeroCodigoPaisEs";
                descripcionError = "Para clientes españoles debe utilizarse NIF, no IDOtro.";
            }
            // 3. Longitud máxima estándar del XSD de TicketBAI (1 a 20 caracteres)
            else if (id.Length is < 1 or > 20)
            {
                codigoError = "Cliente.IdentificacionFiscal.IdOtroExtranjeroLongitudInvalida";
                descripcionError = "El identificador extranjero debe tener entre 1 y 20 caracteres.";
            }
            // 4. Regla específica para NIF-IVA Intracomunitario (Tipo 02)
            else if (tipo == TaxIdType.IntraCommunityVatId && !EsPaisUe(codigoPais))
            {
                codigoError = "Cliente.IdentificacionFiscal.IdOtroExtranjeroNifIvaInvalido";
                descripcionError = $"El tipo NIF-IVA (02) solo es válido para países miembros de la UE. '{codigoPais}' no pertenece a la UE.";
            }
            // 5. Limpieza de caracteres prohibidos en el XML
            else if (TieneCaracteresInvalidosXml(id))
            {
                codigoError = "Cliente.IdentificacionFiscal.IdOtroExtranjeroCaracteresInvalidos";
                descripcionError = "El identificador contiene caracteres no válidos para la firma XML.";
            }
        }
        
        return codigoError.Length == 0;
    }

    public static bool EsPaisUe(string codigoPais)
        => !string.IsNullOrWhiteSpace(codigoPais) && UeCountries.Contains(codigoPais.Trim());

    private static bool TieneCaracteresInvalidosXml(string texto)
    {
        // Comprueba si contiene caracteres fuera del rango ASCII imprimible estándar
        return texto.Any(ch => ch < 32 || ch > 126);
    }

    /// <summary>
    /// Valida un Dni (Nif de persona física española).
    /// Formato: 8 números + letra de control.
    /// </summary>
    public static bool IsDni(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return false;

        valor = valor.Trim().ToUpper();
        var regex = MyDniRegex();
        if (!regex.IsMatch(valor)) return false;

        int numero = int.Parse(valor[..8]);
        char letraEsperada = SpanishTaxIdLetters[numero % 23];
        return valor[^1] == letraEsperada;
    }

    /// <summary>
    /// Valida un Nie (Nif de extranjeros residentes en España).
    /// Formato: X/Y/Z + 7 números + letra de control.
    /// </summary>
    public static bool IsNie(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return false;

        valor = valor.Trim().ToUpper();
        var regex = MyNieRegex();
        if (!regex.IsMatch(valor)) return false;

        // Convertimos la letra inicial a número: X=0, Y=1, Z=2
        int primeraParte = valor[0] switch
        {
            'X' => 0,
            'Y' => 1,
            'Z' => 2,
            _ => -1
        };

        if (primeraParte == -1) return false;

        string numeroCompleto = primeraParte.ToString() + valor.Substring(1, 7);
        int numero = int.Parse(numeroCompleto);

        char letraEsperada = SpanishTaxIdLetters[numero % 23];
        return valor[^1] == letraEsperada;
    }

    /// <summary>
    /// Valida un Cif (Nif de persona jurídica española).
    /// Formato: letra inicial (A-H, J-N, P-S, U-V) + 7 dígitos + dígito de control.
    /// </summary>
    public static bool IsCif(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return false;

        valor = valor.Trim().ToUpper();
        var regex = MyCifRegex();
        return regex.IsMatch(valor);
    }

    [GeneratedRegex(@"^[ABCDEFGHJNPQRSUVW]\d{7}[0-9A-J]$")]
    private static partial Regex MyCifRegex();
    
    [GeneratedRegex(@"^[XYZ]\d{7}[A-Z]$")]
    private static partial Regex MyNieRegex();
    [GeneratedRegex(@"^\d{8}[A-Z]$")]
    private static partial Regex MyDniRegex();
}

