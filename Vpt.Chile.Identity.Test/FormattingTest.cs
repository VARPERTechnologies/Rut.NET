using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vpt.Chile.Identity.Test
{
    public class FormattingTest
    {
        [Test]
        public void NumberOnlyWithoutZeroPadding_ShouldReturn_IntegerPart()
        {
            //Arrange
            RUT rutToTest = RUT.FromString("22222222-2");

            //Act
            string actual = rutToTest.ToString("N");

            //Assert
            Assert.AreEqual("22222222", actual);
        }

        [TestCase("22222222-2", "1", "22222222")]
        [TestCase("22222222-2", "-1", "22222222-1")]
        [TestCase("22222222-2", "0", "22222222")]
        [TestCase("22222222-2", "10", "0022222222")]
        [TestCase("22222222-2", "15", "000000022222222")]
        public void NumberOnlyWithNZeroPadding_ShouldReturn_IntegerPart(string strRut, string padding, string expected)
        {
            //Arrange
            RUT rutToTest = RUT.FromString(strRut);

            //Act
            string actual = rutToTest.ToString($"N{padding}");

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
