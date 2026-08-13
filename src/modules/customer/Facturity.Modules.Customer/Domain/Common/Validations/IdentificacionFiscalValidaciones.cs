using System.Text.RegularExpressions;

namespace Facturity.Modules.Customer.Domain;

public static partial class IdentificacionFiscalValidaciones
{
    private const string LetrasNif = "TRWAGMYFPDXBNJZSQVHLCKE";

    private static readonly HashSet<string> PaisesUe = new(StringComparer.OrdinalIgnoreCase)
    {
        "AT", "BE", "BG", "CY", "CZ", "DE", "DK", "EE", "EL", "FI",
        "FR", "HR", "HU", "IE", "IT", "LT", "LU", "LV", "MT", "NL",
        "PL", "PT", "RO", "SE", "SI", "SK"
    };

    public static bool EsNifValido(string? valor, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(valor))
        {
            codigoError = nameof(NifCodigosError.NifVacio);
            descripcionError = "El Nif está vacío.";
            return false;
        }

        valor = valor.Trim().ToUpper();

        if (valor.Length != 9)
        {
            codigoError = nameof(NifCodigosError.NifLongitudInvalida);
            descripcionError = "El Nif debe tener 9 caracteres.";
            return false;
        }

        // Nif (persona física)
        if (EsDni(valor))
            return EsDniValido(valor, out codigoError, out descripcionError);

        // Nie (extranjero)
        if (EsNie(valor))
            return EsNieValido(valor, out codigoError, out descripcionError);

        // Cif / Nif persona jurídica
        if (EsCif(valor))
            return EsCifValido(valor, out codigoError, out descripcionError);

        codigoError = nameof(NifCodigosError.NifFormatoInvalido);
        descripcionError = "El Nif no tiene un formato o dígito de control válido";
        return false;
    }

    public static bool EsDniValido(string nif, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        nif = nif.ToUpper().Trim();

        if (string.IsNullOrWhiteSpace(nif))
        {
            codigoError = nameof(DniCodigosError.DniVacio);
            descripcionError = "El Dni es vacío.";
            return false;
        }

        if (!int.TryParse(nif[..8], out _))
        {
            codigoError = nameof(DniCodigosError.DniPrimerosCaracteresInvalidos);
            descripcionError = "Los 8 primeros caracteres del Dni son inválidos.";
            return false;
        }

        if (nif.Trim().Length != 9)
        {
            codigoError = nameof(DniCodigosError.DniLongitudInvalida);
            descripcionError = "El Dni debe tener 9 caracteres.";
            return false;
        }

        // Formato Dni estándar: 8 números + letra
        var regex = MyDniRegex();
        if (!regex.IsMatch(nif))
        {
            codigoError = nameof(DniCodigosError.DniFormatoInvalido);
            descripcionError = "El Dni tiene un formato inválido.";
            return false;
        }

        int number = int.Parse(nif[..8]);
        char expectedLetter = LetrasNif[number % 23];
        char actualLetter = nif[^1];

        if (expectedLetter != actualLetter)
        {
            codigoError = nameof(DniCodigosError.DniLetraIncorrecta);
            descripcionError = "La letra del Dni es inválida.";
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool EsNieValido(string nie, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(nie))
        {
            codigoError = nameof(NieCodigosError.NieVacio);
            descripcionError = "El Nie es vacío.";
            return false;
        }

        nie = nie.ToUpper().Trim();

        if (nie.Length != 9)
        {
            codigoError = nameof(NieCodigosError.NieLongitudInvalida);
            descripcionError = "El Nie debe tener 9 caracteres.";
            return false;
        }

        // Formatos permitidos Nie: X/Y/Z + 7 dígitos + letra
        var regex = MyNieRegex();
        if (!regex.IsMatch(nie))
        {
            codigoError = nameof(NieCodigosError.NieFormatoInvalido);
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
            codigoError = nameof(NieCodigosError.NiePrimeraLetraInvalida);
            descripcionError = "La primera letra del Nie es inválida.";
            return false;
        }

        // Completamos con los 7 dígitos siguientes
        numericPart += nie.Substring(1, 7);

        if (!int.TryParse(numericPart, out int number))
        {
            codigoError = nameof(NieCodigosError.NieFormatoInvalido);
            descripcionError = "El Nie tiene un formato inválido.";
            return false;
        }
            
        char expectedLetter = LetrasNif[number % 23];
        char actualLetter = nie[^1];

        if (expectedLetter != actualLetter)
        {
            codigoError = nameof(NieCodigosError.NieUltimaLetraInvalida);
            descripcionError = "La última letra del Nie es inválida.";
            return false;
        }

        return true;
    }

    public static bool EsCifValido(string cif, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(cif))
        {
            codigoError = nameof(CifCodigosError.CifVacio);
            descripcionError = "El Cif (o Nif de persona jurídica) es vacío.";
            return false;
        }

        cif = cif.Trim().ToUpper();

        // Debe tener 9 caracteres exactos
        if (cif.Length != 9)
        {
            codigoError = nameof(CifCodigosError.CifLongitudInvalida);
            descripcionError = "El Cif (o Nif de persona jurídica) debe tener 9 caracteres.";
            return false;
        }

        char letra = cif[0];
        string numeros = cif.Substring(1, 7);
        char control = cif[8];

        // Letra inicial válida
        if (!"ABCDEFGHJKLMNPQRSUVW".Contains(letra))
        {
            codigoError = nameof(CifCodigosError.CifPrimeraLetraInvalida);
            descripcionError = "La primera letra del Cif (o Nif de persona jurídica) es inválida.";
            return false;
        }

        // Los 7 siguientes deben ser dígitos
        if (!numeros.All(char.IsDigit))
        {
            codigoError = nameof(CifCodigosError.CifFormatoInvalido);
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
            codigoError = nameof(CifCodigosError.CifFormatoInvalido);
            descripcionError = "El Cif (o Nif de persona jurídica) tiene un formato inválido.";
            return false;
        }
                
        return true;
    }

    public static bool EsIdOtroExtranjeroValido(
        string codigoPais, 
        TiposIdentificacionFiscal tipo, 
        string id, 
        out string codigoError, 
        out string descripcionError)
    {

        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(codigoPais))
        {
            codigoError = nameof(IdOtroExtranjeroCodigosError.IdOtroExtranjeroCodigoPaisVacio);
            descripcionError = "El código de país es obligatorio.";
        }
        else if (string.IsNullOrWhiteSpace(id))
        {
            codigoError = nameof(IdOtroExtranjeroCodigosError.IdOtroExtranjeroIdVacio);
            descripcionError = "El identificador extranjero no puede estar vacío.";
        }
        else if (tipo == TiposIdentificacionFiscal.Nif)
        {
            codigoError = nameof(IdOtroExtranjeroCodigosError.IdOtroExtranjeroInvalido);
            descripcionError = "El identificador extranjero no puede ser de tipo NIF español.";
        }
        else
        {
            codigoPais = codigoPais.Trim().ToUpper();
            id = id.Trim();

            // 1. Formato ISO-2
            if (codigoPais.Length != 2)
            {
                codigoError = nameof(IdOtroExtranjeroCodigosError.IdOtroExtranjeroCodigoPaisLongitudNoIso2);
                descripcionError = "El código de país debe tener exactamente 2 caracteres ISO-2.";
            }
            // 2. Prohibido utilizar 'ES' en IDOtro
            else if (codigoPais == "ES")
            {
                codigoError = nameof(IdOtroExtranjeroCodigosError.IdOtroExtranjeroCodigoPaisEs);
                descripcionError = "Para clientes españoles debe utilizarse NIF, no IDOtro.";
            }
            // 3. Longitud máxima estándar del XSD de TicketBAI (1 a 20 caracteres)
            else if (id.Length is < 1 or > 20)
            {
                codigoError = nameof(IdOtroExtranjeroCodigosError.IdOtroExtranjeroLongitudInvalida);
                descripcionError = "El identificador extranjero debe tener entre 1 y 20 caracteres.";
            }
            // 4. Regla específica para NIF-IVA Intracomunitario (Tipo 02)
            else if (tipo == TiposIdentificacionFiscal.NifIvaComunitario && !EsPaisUe(codigoPais))
            {
                codigoError = nameof(IdOtroExtranjeroCodigosError.IdOtroExtranjeroNifIvaInvalido);
                descripcionError = $"El tipo NIF-IVA (02) solo es válido para países miembros de la UE. '{codigoPais}' no pertenece a la UE.";
            }
            // 5. Limpieza de caracteres prohibidos en el XML
            else if (TieneCaracteresInvalidosXml(id))
            {
                codigoError = nameof(IdOtroExtranjeroCodigosError.IdOtroExtranjeroCaracteresInvalidos);
                descripcionError = "El identificador contiene caracteres no válidos para la firma XML.";
            }
        }
        
        return codigoError.Length == 0;
    }

    public static bool EsPaisUe(string codigoPais)
        => !string.IsNullOrWhiteSpace(codigoPais) && PaisesUe.Contains(codigoPais.Trim());

    private static bool TieneCaracteresInvalidosXml(string texto)
    {
        // Comprueba si contiene caracteres fuera del rango ASCII imprimible estándar
        return texto.Any(ch => ch < 32 || ch > 126);
    }

    /// <summary>
    /// Valida un Dni (Nif de persona física española).
    /// Formato: 8 números + letra de control.
    /// </summary>
    public static bool EsDni(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return false;

        valor = valor.Trim().ToUpper();
        var regex = MyDniRegex();
        if (!regex.IsMatch(valor)) return false;

        int numero = int.Parse(valor[..8]);
        char letraEsperada = LetrasNif[numero % 23];
        return valor[^1] == letraEsperada;
    }

    /// <summary>
    /// Valida un Nie (Nif de extranjeros residentes en España).
    /// Formato: X/Y/Z + 7 números + letra de control.
    /// </summary>
    public static bool EsNie(string valor)
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

        char letraEsperada = LetrasNif[numero % 23];
        return valor[^1] == letraEsperada;
    }

    /// <summary>
    /// Valida un Cif (Nif de persona jurídica española).
    /// Formato: letra inicial (A-H, J-N, P-S, U-V) + 7 dígitos + dígito de control.
    /// </summary>
    public static bool EsCif(string valor)
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

