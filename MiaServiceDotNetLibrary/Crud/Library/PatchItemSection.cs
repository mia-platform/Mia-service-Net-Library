using System.Collections.Generic;
using MiaServiceDotNetLibrary.Crud.library.enums;
using Newtonsoft.Json;

namespace MiaServiceDotNetLibrary.Crud.library
{
    public class PatchItemSection
    {
        [JsonProperty("filter")] public Dictionary<string, object> PatchFilterSection { get; set; }

        [JsonProperty("update")]
        public Dictionary<PatchCodingKey, Dictionary<string, object>> PatchUpdateSection { get; set; }
    }
}
