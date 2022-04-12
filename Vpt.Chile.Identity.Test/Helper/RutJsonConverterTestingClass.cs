using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vpt.Chile.Identity.Test.Helper
{
    public class RutJsonConverterTestingClass
    {
        [JsonConverter(typeof(RutJsonConverter))]
        public RUT RutField;

        [JsonConverter(typeof(RutJsonConverter))]
        public RUT RutProperty { get; set; }
    }
}
