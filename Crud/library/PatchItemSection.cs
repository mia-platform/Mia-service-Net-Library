using System.Collections.Generic;
using Crud.library.enums;
using Newtonsoft.Json;

namespace Crud.library
{
    public class PatchItemSection
    {
        [JsonProperty("filter")] 
        public  Dictionary<string, object> PatchFilterSection { get; set; }
        [JsonProperty("update")] 
        public Dictionary<PatchCodingKey, Dictionary<string, object>> PatchUpdateSection { get; set; }
    }
}