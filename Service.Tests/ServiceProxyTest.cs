using System.Linq;
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

        [SetUp]
        public void StartMockServer()
        {
            _server = WireMockServer.Start();
        }

        [Test]
        public async Task Should_respond_to_request()
        {
            var initServiceOptions = new InitServiceOptions(_server.Ports.First());
            _sut = new ServiceProxy("localhost", initServiceOptions);

            _server
                .Given(Request.Create().WithPath("/foo").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBody(@"{ ""msg"": ""Hello world!"" }")
                );

            var response = await _sut.Get("/foo");
            var statusCode = (int) response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            Check.That(statusCode).IsEqualTo(200);
            Check.That(responseBody).IsEqualTo(@"{ ""msg"": ""Hello world!"" }");
        }

        [TearDown]
        public void ShutdownServer()
        {
            _server.Stop();
        }
    }
}