using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vpt.Chile.Identity.Exceptions;
using Vpt.Chile.Identity.Test.Helper;

namespace Vpt.Chile.Identity.Test
{
    public class RutJsonConverterTest
    {
        [Test]
        public void Serialize_ValidValues_SerializesOk()
        {
            //Arrange
            RutJsonConverterTestingClass converterHelperClass = new();
            converterHelperClass.RutProperty = RUT.FromNumber(12312313);
            converterHelperClass.RutField = RUT.FromNumber(56564545);
            //Act
            string actual = JObject.FromObject(converterHelperClass).ToString(Newtonsoft.Json.Formatting.None);
            //Assert
            Assert.AreEqual("{\"RutField\":\"56564545-5\",\"RutProperty\":\"12312313-1\"}", actual);
        }
        [TestCase]
        public void Deserialize_InvalidValues_ThrowException()
        {
            //Arrange
            string json = "{\"RutField\":\"56564544-5\",\"RutProperty\":\"12314313-1\"}";
            
            //Act
            
            //Assert
            Assert.Throws<RutException>(() => JObject.Parse(json).ToObject<RutJsonConverterTestingClass>());
        }

        [Test]
        public void Deserialize_ValidValues_ThrowException()
        {
            //Arrange
            string json = "{\"RutField\":\"56564545-5\",\"RutProperty\":\"12312313-1\"}";

            //Act

            //Assert
            Assert.DoesNotThrow(() => JObject.Parse(json));
        }
    }
}
