using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vpt.Chile.Identity.Exceptions;

namespace Vpt.Chile.Identity
{
    public class RUT
    {
        private uint _numero;

        public RUT(uint Numero, char DigitoVerificador)
        {
            if (!IsValid(Numero, DigitoVerificador))
            {
                throw new RutException("Digito verificador no valido");
            }
            Number = Numero;
            VerifierDigit = DigitoVerificador;
        }

        public uint Number
        {
            get { return _numero; }

            set
            {
                if (value > 99999999 || value < 1) throw new ArgumentOutOfRangeException("El numero de RUT o RUN solo puede estar entre 1 y 99.999.999");

                _numero = value;
            }
        }

        public char VerifierDigit { get; set; }

        public static RUT FromString(string rut)
        {
            if (string.IsNullOrEmpty(rut)) throw new ArgumentNullException($"Argumento {nameof(rut)} no puede ser nulo o vacio");
            string notaInitial = "La cadena no contiene un Rut valido";
            string[] rutParts = rut.Split('-');
            uint numero = 0;
            if (string.IsNullOrEmpty(rut)) throw new ArgumentNullException($"La cadena del parametro {nameof(rut)} es vacia o nula. Solo se aceptan cadenas de la forma 12345678-9");
            if (rutParts.Length != 2) throw new ArgumentException($"{notaInitial}. Asegurese de que la cadena tenga exactamente un guion separador");
            if (!uint.TryParse(rutParts[0], out numero)) throw new ArgumentException($"{notaInitial}. Asegurese de que la parte izquierda solo tenga numeros");
            if (rutParts[1].Length != 1) throw new ArgumentException($"{notaInitial}. El digito verificador solo debe tener maximo un caracter");
            if ("0123456789K".Where(c => rutParts[1].Contains(c)).Count() == 0) throw new ArgumentException($"{notaInitial}. El digito verificador solo puede ser una K mayuscula o un numero del 0 al 9");

            return new RUT(numero, rutParts[1][0]);
        }

        public static RUT FromNumber(uint rut)
        {
            if (rut <= 100000 && rut > 999999999) throw new ArgumentOutOfRangeException($"El valor de la parte numerica del rut excede el maximo permitido 999.999.999 o esta por debajo del minimo permitido 100.000"); ;

            return new RUT(rut, CalculateVerifierDigit(rut));
        }

        public static char CalculateVerifierDigit(uint rut)
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
        public string ToString(bool zeroPadding)
        {
            return $"{Number:D8}-{VerifierDigit}";
        }

        public string ToString(string format)
        {
            //TODO: Must use RutFormatInfo to provide format handler
            string.Format(new RutFormatInfo(), format, Number, VerifierDigit);
            throw new NotImplementedException();
        }

        public static implicit operator string(RUT? rut) => rut?.ToString();
        //public static explicit operator RUT(string rut) => RUT.FromString(rut);
        public static implicit operator RUT(string rut) => FromString(rut);
    }
}
