using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vpt.Chile.Identity.Exceptions;

namespace Vpt.Chile.Identity.Test
{
    public class CastingTest
    {
        [TestCase("12345678-6", Reason = "Throws exception due incorrect verifier digit")]
        public void CastingFromStringExceptions(string input)
        {
            //Arrange
            //Act
            //Assert
            Assert.Throws<RutException>(() => { RUT rut = input; });
        }

        [Test]
        public void CastingFromNullStringExceptions()
        {
            //Arrange
            string input = null;
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => { RUT rut = input; });
        }

        [Test]
        public void CastingToString()
        {
            //Arrange
            var rut = new RUT(12345678, '5');
            
            //Act
            string actual = rut;

            //Assert
            Assert.AreEqual("12345678-5", actual);
        }
    }
}
