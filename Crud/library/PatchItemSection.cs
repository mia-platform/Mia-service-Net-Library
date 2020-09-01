using System.Collections.Generic;
using System.Text.Json.Serialization;
using Crud.library.enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Crud.library
{
    public class PatchItemSection
    {
        [JsonProperty("filter")] 
        public  Dictionary<string, JToken> PatchFilterSection { get; set; }
        [JsonProperty("update")] 
        public Dictionary<PatchCodingKey, Dictionary<string, JToken>> PatchUpdateSection { get; set; }
    }
}