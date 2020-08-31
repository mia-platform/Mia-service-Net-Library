using System;
using Crud.library.enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Crud.library
{
    public class TypeSafeEnumJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Console.WriteLine("aa");

            if (!(value is PatchCodingKey))
            {
                JToken t = JToken.FromObject(value);
                t.WriteTo(writer);
            }
            else
            {
                var patchCodingKey = (PatchCodingKey) value;
                JToken token = new JValue(patchCodingKey.Value);
                token.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PatchCodingKey);
        }
    }
}