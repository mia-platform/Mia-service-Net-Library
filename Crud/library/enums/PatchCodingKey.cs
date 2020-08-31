using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Crud.library.enums
{

    public class PatchCodingKey
    {
        private PatchCodingKey(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        [JsonProperty]
        public static PatchCodingKey Set => new PatchCodingKey(@"$set");
        [JsonProperty]
        public static PatchCodingKey SetOnInsert => new PatchCodingKey(@"$setOnInsert");
        [JsonProperty]
        public static PatchCodingKey Unset => new PatchCodingKey(@"$unset");
        [JsonProperty]
        public static PatchCodingKey Inc => new PatchCodingKey(@"$inc");
        [JsonProperty]
        public static PatchCodingKey Mul => new PatchCodingKey(@"$mul");
        [JsonProperty]
        public static PatchCodingKey CurrentDate => new PatchCodingKey(@"$currentDate");

        public override string ToString()
        {
            return Value;
        }
    }
}