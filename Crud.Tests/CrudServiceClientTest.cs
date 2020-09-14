using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Crud.library;
using Crud.library.enums;
using Crud.Tests.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private const HttpStatusCode SuccessStatusCode = HttpStatusCode.OK;
        
        [SetUp]
        public void StartMockServer()
        {
            _server = WireMockServer.Start();
        }

        [Test]
        public async Task TestGet()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

            var user1 = new User(1, "John", "Snow", "Learning things");
            var user2 = new User(2, "Daenerys", "Targaryen", "Riding a dragon");
            var successDeserializedBody = new List<User>() {user1, user2};

            const string successResponseBody =
                @"[{""id"": 1,""firstname"": ""John"",""Lastname"": ""Snow"",""status"": ""Learning things""},{""id"": 2,""firstname"": ""Daenerys"",""Lastname"": ""Targaryen"",""status"": ""Riding a dragon""}]";

            var query = new Dictionary<string, string>{{"foo", "bar"}};

            _server
                .Given(Request.Create().WithPath("/v3/users/").WithParam("foo", "bar").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );

            var result = await _sut.Get<User>(query);

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
        public async Task TestCount()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}");

            _server
                .Given(Request.Create().WithPath("/users/count").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody("42")
                );


            var result = await _sut.Count<User>();

            Check.That(result).IsEqualTo(42);
        }

        [Test]
        public async Task TestExport()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}");

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/users/export").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );

            var result = await _sut.Export<User>();
            var jsonStringResult = await result.ReadAsStringAsync();

            Check.That(jsonStringResult).IsEqualTo(successResponseBody);
        }

        [Test]
        public async Task TestPost()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

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
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

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


        [Test]
        public async Task TestPostValidate()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

            var newUser = new User(3, "Arya", "Stark", "Being no one");

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/validate").UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );

            var result = await _sut.PostValidate<User>(newUser);

            Check.That(result).IsEqualTo(SuccessStatusCode);
        }

        [Test]
        public async Task TestUpsertOne()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

            var newUser = new User(3, "Arya", "Stark", "Being no one");

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/upsert-one").UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );

            var result = await _sut.UpsertOne<User>(newUser);
            var jsonStringResult = await result.ReadAsStringAsync();

            Check.That(jsonStringResult).IsEqualTo(successResponseBody);
        }

        [Test]
        public async Task TestPatch()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

            var updateMapper = new Dictionary<string, object> {{"Lastname", "Targaryen"}};
            var requestBody = new PatchUpdateSection {[PatchCodingKey.Set] = updateMapper};

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/").WithBody(JsonConvert.SerializeObject(requestBody)).UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );


            var result = await _sut.Patch<User>(requestBody);
            var jsonStringResult = await result.ReadAsStringAsync();

            Check.That(jsonStringResult).IsEqualTo(successResponseBody);
        }

        [Test]
        public async Task TestPatchById()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

            var requestBody = new PatchUpdateSection
                {[PatchCodingKey.Set] = new Dictionary<string, object> {{"Lastname", "Targaryen"}}};

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/42").WithBody(JsonConvert.SerializeObject(requestBody)).UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );


            var result = await _sut.PatchById<User>("42", requestBody);
            var jsonStringResult = await result.ReadAsStringAsync();

            Check.That(jsonStringResult).IsEqualTo(successResponseBody);
        }

        [Test]
        public async Task TestPatchBulk()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

            var patchItem1 = new PatchItemSection
            {
                PatchFilterSection = new PatchFilterSection {["foo"] = new JObject {{"foo", "bar"}}},
                PatchUpdateSection = new PatchUpdateSection {[PatchCodingKey.Set] = new Dictionary<string, object> {{"foo", "bar"}}}
            };

            var patchItem2 = new PatchItemSection
            {
                PatchFilterSection = new PatchFilterSection {["baz"] = new JObject {{"foo", "bar"}}},
                PatchUpdateSection = new PatchUpdateSection {[PatchCodingKey.Set] = new Dictionary<string, object> {{"foo", "bar"}}}
            };
            
            var requestBody = new PatchBulkBody {patchItem1, patchItem2};

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/bulk").WithBody(JsonConvert.SerializeObject(requestBody)).UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );


            var result = await _sut.PatchBulk<User>(requestBody);
            var jsonStringResult = await result.ReadAsStringAsync();

            Check.That(jsonStringResult).IsEqualTo(successResponseBody);
        }

        [Test]
        public async Task TestDelete()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/").UsingDelete())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );


            var result = await _sut.Delete<User>();
            var jsonStringResult = await result.ReadAsStringAsync();

            Check.That(jsonStringResult).IsEqualTo(successResponseBody);
        }

        [Test]
        public async Task TestDeleteById()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), $"http://localhost:{_server.Ports.First()}",
                "secret", 3);

            const string successResponseBody =
                @"{""result"":""ok""}";

            _server
                .Given(Request.Create().WithPath("/v3/users/42").UsingDelete())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SuccessStatusCode)
                        .WithBody(successResponseBody)
                );


            var result = await _sut.DeleteById<User>("42");
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
