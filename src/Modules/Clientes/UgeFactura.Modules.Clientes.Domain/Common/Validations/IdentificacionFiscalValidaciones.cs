using System.Text.RegularExpressions;

namespace UgeFactura.Modules.Clientes.Domain;

internal static class IdentificacionFiscalValidaciones
{
    private const string LetrasNif = "TRWAGMYFPDXBNJZSQVHLCKE";

    internal static bool EsNifValido(string valor, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(valor))
        {
            codigoError = NIFCodigosError.NIF_Vacio.ToString();
            descripcionError = "El número de identificación fiscal es vacío.";
            return false;
        }

        valor = valor.Trim().ToUpper();

        if (valor.Length != 9)
        {
            codigoError = NIFCodigosError.NIF_LongitudInvalida.ToString();
            descripcionError = "El número de identificación fiscal debe tener 9 caracteres.";
            return false;
        }

        // NIF (persona física)
        if (EsDNI(valor))
            return EsNifValido(valor, out codigoError, out descripcionError);

        // NIE (extranjero)
        if (EsNie(valor))
            return EsNieValido(valor, out codigoError, out descripcionError);

        // CIF / NIF persona jurídica
        if (EsCif(valor))
            return EsCifValido(valor, out codigoError, out descripcionError);

        codigoError = NIFCodigosError.NIF_FormatoInvalido.ToString();
        descripcionError = "El número de identificación fiscal tiene un formato inválido.";
        return false;
    }

    internal static bool EsDNIValido(string nif, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        nif = nif.ToUpper().Trim();

        if (string.IsNullOrWhiteSpace(nif))
        {
            codigoError = DNICodigosError.DNI_Vacio.ToString();
            descripcionError = "El DNI es vacío.";
            return false;
        }

        if (!int.TryParse(nif[..8], out int _))
        {
            codigoError = DNICodigosError.DNI_PrimerosCaracteresInvalidos.ToString();
            descripcionError = "Los 8 primeros caracteres del DNI son inválidos.";
            return false;
        }

        if (nif.Trim().Length != 9)
        {
            codigoError = DNICodigosError.DNI_LongitudInvalida.ToString();
            descripcionError = "El DNI debe tener 9 caracteres.";
            return false;
        }

        // Formato DNI estándar: 8 números + letra
        var regex = new System.Text.RegularExpressions.Regex(@"^\d{8}[A-Z]$");
        if (!regex.IsMatch(nif))
        {
            codigoError = DNICodigosError.DNI_FormatoInvalido.ToString();
            descripcionError = "El DNI tiene un formato inválido.";
            return false;
        }

        int number = int.Parse(nif.Substring(0, 8));
        char expectedLetter = LetrasNif[number % 23];
        char actualLetter = nif[^1];

        if (expectedLetter != actualLetter)
        {
            codigoError = DNICodigosError.DNI_LetraIncorrecta.ToString();
            descripcionError = "La letra del DNI es inválida.";
            return false;
        }
        else
        {
            return true;
        }
    }

    internal static bool EsNieValido(string nie, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        nie = nie.ToUpper().Trim();

        if (string.IsNullOrWhiteSpace(nie))
        {
            codigoError = NIECodigosError.NIE_Vacio.ToString();
            descripcionError = "El NIE es vacío.";
            return false;
        }

        if (nie.Trim().Length != 9)
        {
            codigoError = NIECodigosError.NIE_LongitudInvalida.ToString();
            descripcionError = "El NIE debe tener 9 caracteres.";
            return false;
        }

        // Formatos permitidos NIE: X/Y/Z + 7 dígitos + letra
        var regex = new System.Text.RegularExpressions.Regex(@"^[XYZ]\d{7}[A-Z]$");
        if (!regex.IsMatch(nie))
        {
            codigoError = NIECodigosError.NIE_FormatoInvalido.ToString();
            descripcionError = "El NIE tiene un formato inválido.";
            return false;
        }

        // Conversión de la letra inicial a número para usar el algoritmo del NIF
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
            codigoError = NIECodigosError.NIE_PrimeraLetraInvalida.ToString();
            descripcionError = "La primera letra del NIE es inválida.";
            return false;
        }

        // Completamos con los 7 dígitos siguientes
        numericPart += nie.Substring(1, 7);

        if (!int.TryParse(numericPart, out int number))
        {
            codigoError = NIECodigosError.NIE_FormatoInvalido.ToString();
            descripcionError = "El NIE tiene un formato inválido.";
            return false;
        }
            
        char expectedLetter = LetrasNif[number % 23];
        char actualLetter = nie[^1];

        if (expectedLetter != actualLetter)
        {
            codigoError = NIECodigosError.NIE_UltimaLetraInvalida.ToString();
            descripcionError = "La última letra del NIE es inválida.";
            return false;
        }

        return true;
    }

    internal static bool EsCifValido(string cif, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(cif))
        {
            codigoError = CIFCodigosError.CIF_Vacio.ToString();
            descripcionError = "El CIF (o NIF de persona jurídica) es vacío.";
            return false;
        }

        cif = cif.Trim().ToUpper();

        // Debe tener 9 caracteres exactos
        if (cif.Length != 9)
        {
            codigoError = CIFCodigosError.CIF_LongitudInvalida.ToString();
            descripcionError = "El CIF (o NIF de persona jurídica) debe tener 9 caracteres.";
            return false;
        }

        char letra = cif[0];
        string numeros = cif.Substring(1, 7);
        char control = cif[8];

        // Letra inicial válida
        if (!"ABCDEFGHJKLMNPQRSUVW".Contains(letra))
        {
            codigoError = CIFCodigosError.CIF_PrimeraLetraInvalida.ToString();
            descripcionError = "La primera letra del CIF (o NIF de persona jurídica) es inválida.";
            return false;
        }

        // Los 7 siguientes deben ser dígitos
        if (!numeros.All(char.IsDigit))
        {
            codigoError = CIFCodigosError.CIF_FormatoInvalido.ToString();
            descripcionError = "El CIF (o NIF de persona jurídica) tiene un formato inválido.";
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

        if (controlDebeSerLetra && control != digitoControlLetra)
        {
            codigoError = CIFCodigosError.CIF_FormatoInvalido.ToString();
            descripcionError = "El CIF (o NIF de persona jurídica) tiene un formato inválido.";
            return false;
        }
        
        if (controlPuedeSerLetraONumero && !(control == digitoControlLetra || control == digitoControlNumero))
        {
            codigoError = CIFCodigosError.CIF_FormatoInvalido.ToString();
            descripcionError = "El CIF (o NIF de persona jurídica) tiene un formato inválido.";
            return false;
        }
        
        if(control != digitoControlNumero)
        {
            codigoError = CIFCodigosError.CIF_FormatoInvalido.ToString();
            descripcionError = "El CIF (o NIF de persona jurídica) tiene un formato inválido.";
            return false;
        }

        return true;
    }

    internal static bool EsIdentificacionNoNifValida(string valor, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(valor))
        {
            codigoError = IdentificacionFiscalCodigosError.ValorIdentificacionFiscal_Vacio.ToString();
            descripcionError = "El valor de la identificación fiscal es vacío.";
            return false;
        }

        if (valor.Length > 20)
        {
            codigoError = IdentificacionFiscalCodigosError.ValorIdentificacionFiscal_LongitudInvalida.ToString();
            descripcionError = "El valor de la identificación fiscal supera los 20 caracteres.";
            return false;
        }

        return true;
    }

    internal static bool EsNifEntidadExtranjeraValido(string valor, out string codigoError, out string descripcionError)
    {
        codigoError = string.Empty;
        descripcionError = string.Empty;

        if (string.IsNullOrWhiteSpace(valor))
        {
            codigoError = OtrosTiposIdentificacionCodigosError.NIFEntidadExtranjera_Vacio.ToString();
            descripcionError = "El NIF de entidad extranjera es vacío.";
            return false;
        }

        if (valor.Length != 8)
        {
            codigoError = OtrosTiposIdentificacionCodigosError.NIFEntidadExtranjera_LongitudInvalida.ToString();
            descripcionError = "El NIF de entidad extranjera debe tener 8 caracteres.";
            return false;
        }

        var regexNifExtranjero = new Regex(@"^N\d{7}[A-Z]?$", RegexOptions.IgnoreCase);
        if (!regexNifExtranjero.IsMatch(valor))
        {
            codigoError = OtrosTiposIdentificacionCodigosError.NIFEntidadExtranjera_FormatoInvalido.ToString();
            descripcionError = "El NIF de entidad extranjera tiene un formato inválido.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Valida un DNI (NIF de persona física española).
    /// Formato: 8 números + letra de control.
    /// </summary>
    internal static bool EsDNI(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return false;

        valor = valor.Trim().ToUpper();
        var regex = new Regex(@"^\d{8}[A-Z]$");
        if (!regex.IsMatch(valor)) return false;

        int numero = int.Parse(valor.Substring(0, 8));
        char letraEsperada = LetrasNif[numero % 23];
        return valor[^1] == letraEsperada;
    }

    /// <summary>
    /// Valida un NIE (NIF de extranjeros residentes en España).
    /// Formato: X/Y/Z + 7 números + letra de control.
    /// </summary>
    internal static bool EsNie(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return false;

        valor = valor.Trim().ToUpper();
        var regex = new Regex(@"^[XYZ]\d{7}[A-Z]$");
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
    /// Valida un CIF (NIF de persona jurídica española).
    /// Formato: letra inicial (A-H, J-N, P-S, U-V) + 7 dígitos + dígito de control.
    /// </summary>
    internal static bool EsCif(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return false;

        valor = valor.Trim().ToUpper();
        var regex = new Regex(@"^[ABCDEFGHJNPQRSUVW]\d{7}[0-9A-J]$");
        if (!regex.IsMatch(valor)) 
            return false;

        // Validación básica de dígito de control
        // Para simplificar aquí solo comprobamos el patrón, puedes añadir cálculo exacto si lo deseas.
        return true;
    }
}

