using System;
using log4net;
using Logging.Entities;
using Moq;
using NUnit.Framework;

namespace Logging.Tests
{
    public class CustomProps
    {
        public string CustomPropA { get; set; }
        public string CustomPropB { get; set; }

        public CustomProps(string customPropA, string customPropB)
        {
            CustomPropA = customPropA;
            CustomPropB = customPropB;
        }
    }
    public class LoggingTest
    {
        private const string Hostname = "mockedHostname";
        private const string Ip = "1.2.3.4";
        private const string HttpRequestMethod = "POST";
        private const string Path = "1.2.3.4/testRoute";
        private const string Original = "Nunit Framework";
        private const long Bytes = 42;
        private const int StatusCode = 200;
        private const decimal ResponseTime = 1337;
        private const int ReqId = 1;
        private const int Level = 30;
        private static readonly long Time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        private RequestLog _mockRequest;
        private Mock<ILog> _mockILog;
        
        [SetUp]
        public void Init()
        {
            _mockRequest = BuildRequestLog();
            _mockILog = new Mock<ILog>();
            _mockILog.Setup(mock => mock.Info(It.IsAny<string>()));
        }
        
        [Test]
        public void TestRequestLog()
        {
            var expectedLog =
                $@"{{""Level"":{Level},""Time"":{Time},""ReqId"":{ReqId},""Http"":{{""Request"":{{""Method"":""{HttpRequestMethod}"",""UserAgent"":{{""Original"":""{Original}""}}}},""Response"":{{""StatusCode"":{StatusCode},""Body"":{{""Bytes"":{Bytes}}}}}}},""Url"":{{""Path"":""{Path}""}},""Host"":{{""Hostname"":""{Hostname}"",""Ip"":""{Ip}""}},""ResponseTime"":{ResponseTime}.0}}";
            var loggingUtility = new LoggingUtility(_mockILog.Object);
            loggingUtility.LogRequest(_mockRequest);
            _mockILog.Verify(mock => mock.Info(expectedLog), Times.Once());
        }

        [Test]
        public void TestMessageLog()
        {
            var requestMocker = new HttpRequestTests();
            var mockRequest = requestMocker.CreateMockRequest();
            // FIXME punteggiatura non coincide e quindi fallisce
            var expectedLog =
                $@"{{""Level"":{Level},""Time"":{Time},""ReqId"":{ReqId},""Msg"":""message"",""CustomPropA"":""foo"",""CustomPropB"":""bar""}}";
            var loggingUtility = new LoggingUtility(_mockILog.Object);
            loggingUtility.LogMessage(mockRequest.Object, LogLevels.Debug, 
                new CustomProps("foo", "bar"), "message");
            _mockILog.Verify(mock => mock.Debug(expectedLog), Times.Once());
        }
        
        private static RequestLog BuildRequestLog()
        {
            return new RequestLog
            {
                Level = Level,
                Time = Time,
                ReqId = ReqId,
                Http = new Http
                {
                    Request = new CpRequest
                    {
                        Method = HttpRequestMethod,
                        UserAgent = new UserAgent
                        {
                            Original = Original
                        },
                    },
                    Response = new CpResponse
                    {
                        Body = new Body
                        {
                            Bytes = Bytes
                        },
                        StatusCode = StatusCode
                    }
                },
                Url = new Url
                {
                    Path = Path,
                },
                Host = new Host
                {
                    Hostname = Hostname,
                    Ip = Ip
                },
                ResponseTime = ResponseTime
            };
        }
    }
}
