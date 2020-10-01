using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Service.Tests
{
    public class ServiceProxyTest
    {
        private WireMockServer _server;
        private ServiceProxy _sut;
        private const int SUCCESS_STATUS_CODE = 200;
        private const string SUCCESS_RESPONSE_BODY = @"{ ""msg"": ""Hello world!"" }";

        [SetUp]
        public void StartMockServer()
        {
            _server = WireMockServer.Start();
        }

        [Test]
        public async Task TestGet()
        {
            var initServiceOptions = new InitServiceOptions(_server.Ports.First());
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);

            _server
                .Given(Request.Create().WithPath("/foo").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var response = await _sut.Get("foo");
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestGetWithCustomInitOptions()
        {
            var initServiceOptions = new InitServiceOptions(_server.Ports.First(), Protocol.Http,
                new Dictionary<string, string> {{"foo", "bar"}}, "prefix");
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);

            _server
                .Given(Request.Create().WithPath("/prefix/foo").WithHeader("foo", "bar").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var response = await _sut.Get("foo");
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestGetWithCustomInitOptionsAndServiceOptions()
        {
            var initServiceOptions = new InitServiceOptions(_server.Ports.First(), Protocol.Http,
                new Dictionary<string, string> {{"foo", "bar"}}, "one");
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);

            _server
                .Given(Request.Create()
                    .WithPath("/one/two/foo")
                    .WithHeader("foo", "bar")
                    .WithHeader("baz", "bam")
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var response = await _sut.Get("foo", "", "",
                new ServiceOptions(_server.Ports.First(), Protocol.Http,
                    new Dictionary<string, string> {{"baz", "bam"}}, "two"));
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestGetWithQueryAndOptions()
        {
            var port = _server.Ports.First();
            var initServiceOptions = new InitServiceOptions(port);
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);

            _server
                .Given(Request.Create().WithPath("/foo").WithParam("bar", "baz").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var headers = new Dictionary<string, string> {{"foo", "bar"}};
            var response = await _sut.Get("foo", "bar=baz", "", new ServiceOptions(port, Protocol.Http, headers));
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(response.RequestMessage.Headers.GetValues("foo").First()).IsEqualTo("bar");
            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestPost()
        {
            var initServiceOptions = new InitServiceOptions(_server.Ports.First());
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);
            const string body = @"{ ""foo"": ""bar"" }";

            _server
                .Given(Request.Create().WithPath("/foo").WithBody(body).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var response = await _sut.Post("foo", "", body);
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestPostWithQueryAndOptions()
        {
            var port = _server.Ports.First();
            var initServiceOptions = new InitServiceOptions(port);
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);
            const string body = @"{ ""foo"": ""bar"" }";

            _server
                .Given(Request.Create().WithPath("/foo").WithParam("bar", "baz").WithBody(body).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var headers = new Dictionary<string, string> {{"foo", "bar"}};
            var response = await _sut.Post("foo", "bar=baz", body, new ServiceOptions(port, Protocol.Http, headers));
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(response.RequestMessage.Headers.GetValues("foo").First()).IsEqualTo("bar");
            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestPut()
        {
            var initServiceOptions = new InitServiceOptions(_server.Ports.First());
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);
            const string body = @"{ ""foo"": ""bar"" }";

            _server
                .Given(Request.Create().WithPath("/foo").WithBody(body).UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var response = await _sut.Put("foo", "", body);
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestPutWithQueryAndOptions()
        {
            var port = _server.Ports.First();
            var initServiceOptions = new InitServiceOptions(port);
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);
            const string body = @"{ ""foo"": ""bar"" }";

            _server
                .Given(Request.Create().WithPath("/foo").WithParam("bar", "baz").WithBody(body).UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var headers = new Dictionary<string, string> {{"foo", "bar"}};
            var response = await _sut.Put("foo", "bar=baz", body, new ServiceOptions(port, Protocol.Http, headers));
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(response.RequestMessage.Headers.GetValues("foo").First()).IsEqualTo("bar");
            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestPatch()
        {
            var initServiceOptions = new InitServiceOptions(_server.Ports.First());
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);
            const string body = @"{ ""foo"": ""bar"" }";

            _server
                .Given(Request.Create().WithPath("/foo").WithBody(body).UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var response = await _sut.Patch("foo", "", body);
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestPatchWithQueryAndOptions()
        {
            var port = _server.Ports.First();
            var initServiceOptions = new InitServiceOptions(port);
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);
            const string body = @"{ ""foo"": ""bar"" }";

            _server
                .Given(Request.Create().WithPath("/foo").WithParam("bar", "baz").WithBody(body).UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var headers = new Dictionary<string, string> {{"foo", "bar"}};
            var response = await _sut.Patch("foo", "bar=baz", body, new ServiceOptions(port, Protocol.Http, headers));
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(response.RequestMessage.Headers.GetValues("foo").First()).IsEqualTo("bar");
            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestDelete()
        {
            var initServiceOptions = new InitServiceOptions(_server.Ports.First());
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);

            _server
                .Given(Request.Create().WithPath("/foo").UsingDelete())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var response = await _sut.Delete("foo", "foo=bar");
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [Test]
        public async Task TestDeleteWithQueryAndOptions()
        {
            var port = _server.Ports.First();
            var initServiceOptions = new InitServiceOptions(port);
            _sut = new ServiceProxy(new Dictionary<string, string>(), "localhost", initServiceOptions);

            _server
                .Given(Request.Create().WithPath("/foo").WithParam("bar", "baz").UsingDelete())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(SUCCESS_STATUS_CODE)
                        .WithBody(SUCCESS_RESPONSE_BODY)
                );

            var headers = new Dictionary<string, string> {{"foo", "bar"}};
            var response = await _sut.Delete("foo", "bar=baz", "", new ServiceOptions(port, Protocol.Http, headers));
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(response.RequestMessage.Headers.GetValues("foo").First()).IsEqualTo("bar");
            Check.That(statusCode).IsEqualTo(SUCCESS_STATUS_CODE);
            Check.That(responseBody).IsEqualTo(SUCCESS_RESPONSE_BODY);
        }

        [TearDown]
        public void ShutdownServer()
        {
            _server.Stop();
        }
    }
}
