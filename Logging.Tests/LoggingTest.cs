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
        private static readonly long Time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        private CompletedRequestLog _mockCompletedRequest;
        private Mock<ILog> _mockILog;
        
        [SetUp]
        public void Init()
        {
            _mockCompletedRequest = BuildRequestLog();
            _mockILog = new Mock<ILog>();
            _mockILog.Setup(mock => mock.Info(It.IsAny<string>()));
            _mockILog.Setup(mock => mock.Debug(It.IsAny<string>()));
            Logger.SetInstance(_mockILog.Object);
        }
        
        [Test]
        public void TestCompletedRequestLog()
        {
            var expectedLog =
                $@"{{""level"":{(int) LogLevels.Info},""time"":{Time},""reqId"":{ReqId},""http"":{{""request"":{{""method"":""{HttpRequestMethod}"",""userAgent"":{{""original"":""{Original}""}}}},""response"":{{""statusCode"":{StatusCode},""body"":{{""bytes"":{Bytes}}}}}}},""url"":{{""path"":""{Path}""}},""host"":{{""hostname"":""{Hostname}"",""ip"":""{Ip}""}},""responseTime"":{ResponseTime}.0}}";
            Logger.LogRequest(_mockCompletedRequest);
            _mockILog.Verify(mock => mock.Info(expectedLog), Times.Once());
        }

        [Test]
        public void TestMessageLog()
        {
            var requestMocker = new HttpRequestTests();
            var mockRequest = requestMocker.CreateMockRequest();
            var logBeginsWith = $@"{{""level"":{(int) LogLevels.Debug},""time"":";
            var logEndsWith = 
                $@",""reqId"":{ReqId},""msg"":""message"",""customPropA"":""foo"",""customPropB"":""bar""}}";
            Logger.Debug(mockRequest.Object, 
                "message", new CustomProps("foo", "bar"));
            _mockILog.Verify(mock => mock.Debug(It.Is<string>(str =>
                str.StartsWith(logBeginsWith) && str.EndsWith(logEndsWith))), Times.Once);
        }
        
        [Test]
        public void TestMessageLogWithoutParams()
        {
            var requestMocker = new HttpRequestTests();
            var mockRequest = requestMocker.CreateMockRequest();
            var logBeginsWith = $@"{{""level"":{(int) LogLevels.Warn},""time"":";
            var logEndsWith = 
                $@",""reqId"":{ReqId},""msg"":""message""}}";
            Logger.Warn(mockRequest.Object, "message");
            _mockILog.Verify(mock => mock.Warn(It.Is<string>(str =>
                str.StartsWith(logBeginsWith) && str.EndsWith(logEndsWith))), Times.Once);
        }

        private static CompletedRequestLog BuildRequestLog()
        {
            return new CompletedRequestLog
            {
                Level = (int) LogLevels.Info,
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
