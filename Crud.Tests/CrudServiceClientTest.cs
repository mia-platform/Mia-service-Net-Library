using System.Collections.Generic;
using System.Linq;
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
        private const int SuccessStatusCode = 200;


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

        [SetUp]
        public void StartMockServer()
        {
            _server = WireMockServer.Start();
        }

        [Test]
        public async Task TestGet()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}", "secret", 3);

            var user1 = new User(1, "John", "Snow", "Learning things");
            var user2 = new User(2, "Daenerys", "Targaryen", "Riding a dragon");
            var successDeserializedBody = new List<User>() {user1, user2};

            const string successResponseBody =
                @"[{""id"": 1,""firstname"": ""John"",""Lastname"": ""Snow"",""status"": ""Learning things""},{""id"": 2,""firstname"": ""Daenerys"",""Lastname"": ""Targaryen"",""status"": ""Riding a dragon""}]";

            _server
                .Given(Request.Create().WithPath("/v3/users/").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );

            var result = await _sut.Get<User>();

            Check.That(result.Count).IsEqualTo(successDeserializedBody.Count);
            Check.That(result[0].Lastname).IsEqualTo("Snow");
            Check.That(result[1].Lastname).IsEqualTo("Targaryen");
        }

        [Test]
        public async Task TestGetById()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}");

            var user = new User(2, "Daenerys", "Targaryen", "Riding a dragon");

            const string successResponseBody =
                @"{""id"": 2,""firstname"": ""Daenerys"",""Lastname"": ""Targaryen"",""status"": ""Riding a dragon""}";

            _server
                .Given(Request.Create().WithPath("/users/2").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );


            var result = await _sut.GetById<User>("2");

            Check.That(result.Id).IsEqualTo(user.Id);
            Check.That(result.Firstname).IsEqualTo(user.Firstname);
            Check.That(result.Lastname).IsEqualTo(user.Lastname);
            Check.That(result.Status).IsEqualTo(user.Status);
        }

        [Test]
        public async Task TestPost()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}", "secret", 3);

            var newUser = new User(3, "Arya", "Stark", "Being no one");

            const string successResponseBody =
                @"{""id"":3,""firstname"":""Arya"",""Lastname"":""Stark"",""status"":""Being no one""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/").UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );

            var result = await _sut.Post(newUser);
            var jsonStringResult = await result.ReadAsStringAsync();

            Check.That(jsonStringResult).IsEqualTo(successResponseBody);
        }

        [Test]
        public async Task TestPostBulk()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}", "secret", 3);

            var newUser1 = new User(1, "John", "Snow", "Learning things");
            var newUser2 = new User(2, "Daenerys", "Targaryen", "Riding a dragon");
            var newUser3 = new User(3, "Arya", "Stark", "Being no one");
            var newUsers = new List<User> {newUser1, newUser2, newUser3};

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/bulk").UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );

            var result = await _sut.PostBulk(newUsers);
            var jsonStringResult = await result.ReadAsStringAsync();

            Check.That(jsonStringResult).IsEqualTo(successResponseBody);
        }

        [TearDown]
        public void ShutdownServer()
        {
            _server.Stop();
        }
    }
}