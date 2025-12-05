using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgeFactura.Domain.ValueObjects
{
    public sealed record Nif
    {
        public string Value { get; }

        public Nif(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El NIF no puede estar vacío.", nameof(value));

            value = value.Trim().ToUpper();

            if (!IsValid(value))
                throw new ArgumentException($"El NIF '{value}' no es válido.", nameof(value));

            Value = value;
        }

        private static bool IsValid(string nif)
        {
            // Formato DNI estándar: 8 números + letra
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{8}[A-Z]$");
            if (!regex.IsMatch(nif))
                return false;

            // Cálculo de la letra correcta
            const string letters = "TRWAGMYFPDXBNJZSQVHLCKE";

            int number = int.Parse(nif.Substring(0, 8));
            char expectedLetter = letters[number % 23];
            char actualLetter = nif[^1];

            return expectedLetter == actualLetter;
        }

        public override string ToString() => Value;
    }

}
