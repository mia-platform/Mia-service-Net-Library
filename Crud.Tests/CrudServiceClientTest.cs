using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NFluent;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Crud.Tests
{
    public class Tests
    {
        private WireMockServer _server;
        private CrudServiceClient _sut;
        private HttpRequestHeaders _httpRequestHeaders = null;
        private const int SUCCESS_STATUS_CODE = 200;


        [JsonObject("users")]
        private class User
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

        private List<User> _successDeserializedBody = new List<User>();
        private User _user1 = new User(1, "John", "Snow", "Learning things");
        private User _user2 = new User(2, "Daenerys", "Targaryen", "Riding a dragon");

        [SetUp]
        public void StartMockServer()
        {
            _server = WireMockServer.Start();

            _successDeserializedBody.Add(_user1);
            _successDeserializedBody.Add(_user2);
        }

        [Test]
        public async Task TestGet()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), "http://localhost:3001", "secret", 3);
            
            const string SUCCESS_RESPONSE_BODY = @"
                [
                  {
                    ""id"": 1,
                    ""firstname"": ""John"",
                    ""Lastname"": ""Snow"",
                    ""status"": ""Learning things""
                  },
                  {
                    ""id"": 2,
                    ""firstname"": ""Daenerys"",
                    ""Lastname"": ""Targaryen"",
                    ""status"": ""Riding a dragon""
                  }
                ]";
            
            _server
                .Given(Request.Create().WithPath("/v3/users/").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var result = await _sut.Get<User>();

            Check.That(result.Count).IsEqualTo(_successDeserializedBody.Count);
            Check.That(result[0].Lastname).IsEqualTo("Snow");
            Check.That(result[1].Lastname).IsEqualTo("Targaryen");
        }

        [Test]
        public async Task TestGetById()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), "http://localhost:3001");

            const string SUCCESS_RESPONSE_BODY = @"
                  {
                    ""id"": 2,
                    ""firstname"": ""Daenerys"",
                    ""Lastname"": ""Targaryen"",
                    ""status"": ""Riding a dragon""
                  }";
            
            _server
                .Given(Request.Create().WithPath("/users/2").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );


            var result = await _sut.GetById<User>("2");

            Check.That(result.Id).IsEqualTo(_user2.Id);
            Check.That(result.Firstname).IsEqualTo(_user2.Firstname);
            Check.That(result.Lastname).IsEqualTo(_user2.Lastname);
            Check.That(result.Status).IsEqualTo(_user2.Status);
        }

        [TearDown]
        public void ShutdownServer()
        {
            _server.Stop();
        }
    }
}    