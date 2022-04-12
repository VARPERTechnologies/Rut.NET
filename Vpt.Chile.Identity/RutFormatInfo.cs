using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Vpt.Chile.Identity
{
    //TODO:Must implement
    public class RutFormatInfo : IFormatProvider, ICustomFormatter
    {
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null) return "null";

            if (arg is not RUT) throw new FormatException(string.Format("The format of '{0}' is invalid.", format));

            RUT inputRut = (RUT)arg;
            return Format(inputRut, format);
        }

        private string Format(RUT inputRut, string? format)
        {
            StringBuilder result = new(format);

            Regex numFormatRegex = new(@"N(\d*)");
            var matches = numFormatRegex.Matches(format);

            foreach (Match match in matches)
            {
                string length = match.Groups[1].Value;
                result.Replace(match.Value, string.Format("{0:D" + length + "}", inputRut.Number));
            }

            result.Replace('V', inputRut.VerifierDigit);
            //var vOccurrences = format.Where(c => c == 'V');
            //foreach (var vS in vOccurrences)
            //{
            //    ;
            //}
            //else if (format == "M")
            //{
            result.Replace("M", inputRut.Number.ToString("N", new CultureInfo("es-CL")) + "-" + inputRut.VerifierDigit);
            //}
            //else
            //{
            //    throw new FormatException(string.Format("The format of '{0}' is invalid.", format));
            //}
            return result.ToString();
        }

        public object? GetFormat(Type? formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }
    }
}
