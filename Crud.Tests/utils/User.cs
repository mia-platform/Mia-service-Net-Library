using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Crud.Tests.utils
{
    [JsonObject("users")]
    internal class User
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("firstname")] public string Firstname { get; set; }
        [JsonPropertyName("Lastname")] public string Lastname { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; }

        public User()
        {
        }

        public User(int id, string firstname, string lastname, string status)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Status = status;
        }
    }

}