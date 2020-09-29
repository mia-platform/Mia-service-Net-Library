using System;
using log4net;
using Logging.Entities;
using Moq;
using NUnit.Framework;

namespace Logging.Tests
{
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
        
        [Test]
        public void TestRequestLog()
        {
            var expectedLog =
                $@"{{""Level"":{Level},""Time"":{Time},""ReqId"":{ReqId},""Http"":{{""Request"":{{""Method"":""{HttpRequestMethod}"",""UserAgent"":{{""Original"":""{Original}""}}}},""Response"":{{""StatusCode"":{StatusCode},""Body"":{{""Bytes"":{Bytes}}}}}}},""Url"":{{""Path"":""{Path}""}},""Host"":{{""Hostname"":""{Hostname}"",""Ip"":""{Ip}""}},""ResponseTime"":{ResponseTime}.0}}";
            var mockRequest = BuildCompletedRequestLog();
            var mockILog = new Mock<ILog>();
            var loggingUtility = new LoggingUtility(mockILog.Object);
            mockILog.Setup(mock => mock.Info(It.IsAny<string>()));
            loggingUtility.LogRequest(mockRequest);
            mockILog.Verify(mock => mock.Info(expectedLog), Times.Once());
        }

        private static RequestLog BuildCompletedRequestLog()
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
