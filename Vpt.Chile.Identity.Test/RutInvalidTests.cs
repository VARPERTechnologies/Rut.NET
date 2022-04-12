using NUnit.Framework;
using System;
using Vpt.Chile.Identity.Exceptions;

namespace Vpt.Chile.Identity.Test
{
    public class RutInvalidTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase((uint)123456789, '1')]
        [TestCase(uint.MaxValue, '1')]
        [TestCase(uint.MinValue, '1')]
        public void Constructor_WrongRut_ThrowException(uint number, char digit)
        {
            Assert.Throws<RutException>(() => new RUT(number, digit));
        }

        [TestCase(uint.MinValue, '1')]
        [TestCase(uint.MaxValue, '1')]
        [TestCase((uint)0, '1')]
        [TestCase((uint)22222222, '1')]
        [TestCase((uint)5476734, 'k')]
        public void IsValid_ShouldBe_False(uint number, char verifierDigit)
        {
            Assert.IsFalse(RUT.IsValid(number, verifierDigit));
        }

        [TestCase("12345678--9")]
        [TestCase("123456789")]
        [TestCase("1234567A-9")]
        [TestCase("12345678-k")]
        [TestCase("ADASDSDDAS")]
        [TestCase("12345678912-5")]
        public void FromString_WronString_ThrowException(string cases)
        {
            Assert.Throws<ArgumentException>(() => RUT.FromString(cases));
        }

        [Test]
        public void FromString_NullString_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => RUT.FromString(null));
        }

    }
}