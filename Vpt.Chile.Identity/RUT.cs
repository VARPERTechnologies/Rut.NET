using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Vpt.Chile.Identity.Exceptions;

namespace Vpt.Chile.Identity
{
    public class RUT
    {
        private uint _numero;
        private char _verifierDigit;

        public RUT(uint Numero, char DigitoVerificador)
        {
            if (!IsValid(Numero, DigitoVerificador))
            {
                throw new RutException("Digito verificador no valido");
            }
            _numero = Numero;
            _verifierDigit = DigitoVerificador;
        }

        public uint Number { get { return _numero; } }

        public char VerifierDigit { get { return _verifierDigit; } }

        /// <summary>
        /// Create a RUT instance from a valid RUT or RUN string formatted like "12345678-9".
        /// Anything with a different format is not parsed correctly
        /// </summary>
        /// <param name="rut">A valid string with number part between (exclusive) 100000 and 999999999 (inclusive) and a valid verifier digit</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>}
        public static RUT FromString(string rut)
        {
            //TODO: Overload this function to support custom rut formats
            if (string.IsNullOrEmpty(rut)) throw new ArgumentNullException($"{nameof(rut)} no puede ser nulo o vacio");
            string notaInitial = "La cadena no contiene un Rut valido";
            string[] rutParts = rut.Split('-');
            uint numero = 0;
            if (rutParts.Length != 2) throw new ArgumentException($"{notaInitial}. Asegurese de que la cadena tenga exactamente un guion separador");
            if (!uint.TryParse(rutParts[0], out numero)) throw new ArgumentException($"{notaInitial}. Asegurese de que la parte izquierda solo tenga numeros");
            if (rutParts[1].Length != 1) throw new ArgumentException($"{notaInitial}. El digito verificador solo debe tener maximo un caracter");
            if ("0123456789K".Where(c => rutParts[1].Contains(c)).Count() == 0) throw new ArgumentException($"{notaInitial}. El digito verificador solo puede ser una K mayuscula o un numero del 0 al 9");

            return new RUT(numero, rutParts[1][0]);
        }

        /// <summary>
        /// Create an instance of RUT class from a valid number, the verifier digit is calculated implicitly
        /// </summary>
        /// <param name="rut">A valid number between (exclusive) 100000 and 999999999 (inclusive)</param>
        /// <returns>An instance of <code>RUT</code> class </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static RUT FromNumber(uint rut)
        {
            if (rut <= 100000 && rut > 999999999) throw new ArgumentOutOfRangeException(paramName: nameof(rut), message: $"El valor de la parte numerica del rut excede el maximo permitido 999.999.999 o esta por debajo del minimo permitido 100.000"); ;

            return new RUT(rut, CalculateVerifierDigit(rut));
        }

        private static char CalculateVerifierDigit(uint rut)
        {
            uint suma = 0;
            uint multiplicador = 1;
            while (rut != 0)
            {
                multiplicador++;
                if (multiplicador == 8)
                    multiplicador = 2;
                suma += rut % 10 * multiplicador;
                rut = rut / 10;
            }
            suma = 11 - suma % 11;
            if (suma == 11)
            {
                return '0';
            }
            else if (suma == 10)
            {
                return 'K';
            }
            else
            {
                return suma.ToString()[0];
            }
        }

        public static bool IsValid(uint Numero, char VerifierDigit)
        {
            if (Numero <= 100000) return false;

            char digit = CalculateVerifierDigit(Numero);
            return VerifierDigit == digit;
        }

        public static bool IsValid(string Rut)
        {
            try
            {
                RUT tempRut = FromString(Rut);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{Number}-{VerifierDigit}";
        }
        /// <summary>
        /// 8-digits zero padded string version of a rut.
        /// This representation is usefull for fixed lenght identifiers
        /// </summary>
        /// <param name="zeroPadding"></param>
        /// <returns></returns>
        [Obsolete("This method should be replaced for ToString(string format) which is more robust and flexible")]
        public string ToString(bool zeroPadding)
        {
            return $"{Number:D8}-{VerifierDigit}";
        }

        public string ToString(string format)
        {
            //TODO: Must use RutFormatInfo to provide format handler
            return Format(format);
        }

        private string Format(string format)
        {
            //TODO: Add support for escaped single quotes
            if (format.Count(c => c == '\'') % 2 != 0)
                throw new ArgumentException($"The format \"{format}\" is invalid. There are missing single quotes");

            StringBuilder result = new(format);

            Regex numFormatRegex = new(@"N(\d*)");
            var matches = numFormatRegex.Matches(format);

            foreach (Match match in matches)
            {
                //TODO: If possible, add test case to support integer overflow (strings reaaaallyyyyy long)
                if (BetweenQuotes(format, match)) continue;

                string length = match.Groups[1].Value;
                result.Replace(match.Value, string.Format("{0:D" + length + "}", Number));
            }

            if (!BetweenQuotes(format, 'V'))
                result.Replace('V', VerifierDigit);

            if(!BetweenQuotes(format, 'M'))
                result.Replace("M", Number.ToString("#,0", new CultureInfo("es-CL")) + "-" + VerifierDigit);

            RemoveQuotes(result);
            return result.ToString();
        }

        private void RemoveQuotes(StringBuilder format)
        {
            format.Replace("'", null);
        }

        private bool BetweenQuotes(string format, Match match)
        {
            var indexes = GetIndexes(format, '\'');

            for(int i = 0; i < indexes.Count; i++)
            {
                if (match.Groups[0].Index > indexes[i] &&
                    match.Groups[0].Index < indexes[Math.Min(indexes.Count - 1, i + 1)])
                
                    return true;
                
            }
            return false;
        }

        private bool BetweenQuotes(string format, char c)
        {
            var indexes = GetIndexes(format, '\'');
            var indexOfChar = format.IndexOf(c);

            for (int i = 0; i < indexes.Count; i++)
            {
                if (indexOfChar > indexes[i] &&
                    indexOfChar < indexes[Math.Min(indexes.Count - 1, i + 1)])
                    return true;
            }
            return false;
        }

        private List<int> GetIndexes(string s, char c)
        {
            var foundIndexes = new List<int>();

            // for loop end when i=-1 ('a' not found)
            for (int i = s.IndexOf(c); i > -1; i = s.IndexOf(c, i + 1))
            {
                foundIndexes.Add(i);
            }
            return foundIndexes;
        }

        public static implicit operator string(RUT? rut) => rut?.ToString();
        public static implicit operator RUT(string rut) => FromString(rut);
    }
}
