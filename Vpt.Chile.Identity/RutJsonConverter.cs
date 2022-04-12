using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vpt.Chile.Identity
{
    /// <summary>
    /// Performs conversions between RUT and string types to and from json objects
    /// </summary>
    /// <remarks>TODO: Support and test for methods and functions. It actually works only with fields and properties</remarks>
    public class RutJsonConverter : JsonConverter<RUT>
    {
        public override void WriteJson(JsonWriter writer, RUT? value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override RUT? ReadJson(JsonReader reader, Type objectType, RUT? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;

            return RUT.FromString(s);
        }
    }
}
