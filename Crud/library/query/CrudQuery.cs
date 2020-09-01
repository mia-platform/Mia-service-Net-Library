using System.Collections.Generic;
using Crud.library.enums;
using Newtonsoft.Json;

namespace Crud.library.query
{
    public class CrudQuery
    {
        [JsonProperty("_id")] public string Id { get; set; }
        [JsonProperty("creatorId")] public string CreatorId { get; set; }
        [JsonProperty("createdAt")] public string CreatedAt { get; set; }
        [JsonProperty("updaterId")] public string UpdaterId { get; set; }
        [JsonProperty("updatedAt")] public string UpdatedAt { get; set; }
        [JsonProperty("_q")] public string MongoQuery { get; set; }
        [JsonProperty("_st")] public State State { get; set; }
        [JsonProperty("_p")] public List<string> Properties { get; set; }
        [JsonProperty("_l")] public int Limit { get; set; }
        [JsonProperty("_sk")] public int Skip { get; set; }
        [JsonProperty("_s")] public string SortBy { get; set; }
    }
}