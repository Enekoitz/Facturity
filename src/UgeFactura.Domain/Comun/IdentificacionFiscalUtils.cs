using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UgeFactura.Domain.Cliente.ValueObjects;

namespace UgeFactura.Domain.Comun
{
    public static class IdentificacionFiscalUtils
    {
        private const string LetrasNif = "TRWAGMYFPDXBNJZSQVHLCKE";

        public static bool EsNifNieOCifValido(string valor, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(valor))
            {
                mensajeError = "El NIF/NIE/CIF no puede estar vacío.";
                return false;
            }

            valor = valor.Trim().ToUpper();

            if (valor.Length != 9)
            {
                mensajeError = "El NIF/NIE/CIF debe tener una longitud de 9 caracteres.";
                return false;
            }

            // NIF (persona física)
            if (EsNif(valor))
                return EsNifValido(valor, out mensajeError);

            // NIE (extranjero)
            if (EsNie(valor))
                return EsNieValido(valor, out mensajeError);

            // CIF / NIF persona jurídica
            if (EsCif(valor))
                return EsCifValido(valor, out mensajeError);

            mensajeError = "El formato del NIF/NIE/CIF no es válido.";
            return false;
        }

        public static bool EsNifValido(string nif, out string mensajeError)
        {
            mensajeError = string.Empty;
            nif = nif.ToUpper().Trim();

            if (string.IsNullOrWhiteSpace(nif))
            {
                mensajeError = "El NIF no puede estar vacío.";
                return false;
            }

            if (!int.TryParse(nif[..8], out int numero))
            {
                mensajeError = "Los 8 primeros caracteres del NIF deben ser numéricos.";
                return false;
            }

            if (nif.Trim().Length != 9)
            {
                mensajeError = "El NIF debe tener una longitud de 9 caracteres.";
                return false;
            }

            // Formato DNI estándar: 8 números + letra
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{8}[A-Z]$");
            if (!regex.IsMatch(nif))
            {
                mensajeError = "El formato del NIF no es correcto.";
                return false;
            }

            int number = int.Parse(nif.Substring(0, 8));
            char expectedLetter = LetrasNif[number % 23];
            char actualLetter = nif[^1];

            if (expectedLetter != actualLetter)
            {
                mensajeError = "La letra del NIF no es correcta.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool EsNieValido(string nie, out string mensajeError)
        {
            mensajeError = string.Empty;

            nie = nie.ToUpper().Trim();

            if (string.IsNullOrWhiteSpace(nie))
            {
                mensajeError = "El NIE no puede estar vacío.";
                return false;
            }

            if (nie.Trim().Length != 9)
            {
                mensajeError = "El NIE debe tener una longitud de 9 caracteres.";
                return false;
            }

            // Formatos permitidos NIE: X/Y/Z + 7 dígitos + letra
            var regex = new System.Text.RegularExpressions.Regex(@"^[XYZ]\d{7}[A-Z]$");
            if (!regex.IsMatch(nie))
            {
                mensajeError = "El formato del NIE no es correcto.";
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
                mensajeError = "La primera letra del NIE no es correcta.";
                return false;
            }

            // Completamos con los 7 dígitos siguientes
            numericPart += nie.Substring(1, 7);

            if (!int.TryParse(numericPart, out int number))
                return false;

            char expectedLetter = LetrasNif[number % 23];
            char actualLetter = nie[^1];

            if (expectedLetter != actualLetter)
            {
                mensajeError = "La última letra del NIE no es correcta.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool EsCifValido(string cif, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(cif))
            {
                mensajeError = "El NIF de persona jurídica no puede estar vacío.";
                return false;
            }

            cif = cif.Trim().ToUpper();

            // Debe tener 9 caracteres exactos
            if (cif.Length != 9)
            {
                mensajeError = "El NIF de persona jurídica debe tener una longitud de 9 caracteres.";
                return false;
            }

            char letra = cif[0];
            string numeros = cif.Substring(1, 7);
            char control = cif[8];

            // Letra inicial válida
            if (!"ABCDEFGHJKLMNPQRSUVW".Contains(letra))
            {
                mensajeError = "El NIF de persona jurídica debe iniciar con una letra válida.";
                return false;
            }

            // Los 7 siguientes deben ser dígitos
            if (!numeros.All(char.IsDigit))
            {
                mensajeError = "El NIF de persona jurídica debe incluir 7 dígitos a partir del segundo caracter.";
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

            if (controlDebeSerLetra)
            {
                if ( control != digitoControlLetra)
                {
                    mensajeError = "El dígito de control del NIF de persona jurídica debe ser una letra.";
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (controlPuedeSerLetraONumero)
            {
                if (control == digitoControlLetra || control == digitoControlNumero)
                {
                    return true;
                }
                else
                {
                    mensajeError = "El dígito de control del NIF de persona jurídica debe ser una letra o un número.";
                    return false;
                }
            }

            // resto → debe ser número
            if(control == digitoControlNumero)
            {
                return true;
            }
            else
            {
                mensajeError = "El dígito de control del NIF de persona jurídica es inválido.";
                return false;
            }
        }

        public static bool EsIdentificacionNoNifValida(string valor, string nombreTipo, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(valor))
            {
                mensajeError = $"El tipo de identificación '{nombreTipo}' no puede estar vacía.";
                return false;
            }

            if (valor.Length > 20)
            {
                mensajeError = $"El tipo de identificación '{nombreTipo}' no puede tener más de 20 caracteres.";
                return false;
            }

            return true;
        }

        public static bool EsNifEntidadExtranjeraValido(string valor, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(valor))
            {
                mensajeError = "El NIF de entidad extranjera no puede estar vacío.";
                return false;
            }

            if (valor.Length != 8)
            {
                mensajeError = "El NIF de entidad extranjera debe tener 8 caracteres.";
                return false;
            }

            var regexNifExtranjero = new Regex(@"^N\d{7}[A-Z]?$", RegexOptions.IgnoreCase);
            if (!regexNifExtranjero.IsMatch(valor))
            {
                mensajeError = "El formato del NIF de entidad extranjera no es válido.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida un NIF de persona física española (DNI).
        /// Formato: 8 números + letra de control.
        /// </summary>
        public static bool EsNif(string valor)
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
        /// Valida un NIE (extranjeros residentes en España).
        /// Formato: X/Y/Z + 7 números + letra de control.
        /// </summary>
        public static bool EsNie(string valor)
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
        /// Valida un CIF de persona jurídica española.
        /// Formato: letra inicial (A-H, J-N, P-S, U-V) + 7 dígitos + dígito de control.
        /// </summary>
        public static bool EsCif(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;

            valor = valor.Trim().ToUpper();
            var regex = new Regex(@"^[ABCDEFGHJNPQRSUVW]\d{7}[0-9A-J]$");
            if (!regex.IsMatch(valor)) return false;

            // Validación básica de dígito de control
            // Para simplificar aquí solo comprobamos el patrón, puedes añadir cálculo exacto si lo deseas.
            return true;
        }
    }
}

