using System;
using System.Collections.Generic;
using log4net;
using MiaServiceDotNetLibrary.Logging;
using MiaServiceDotNetLibrary.Logging.Entities;
using Moq;
using NUnit.Framework;

namespace MiaServiceDotNetLibrary.Tests.Logging
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
        private const string ForwardedHostname = "mockedForwardedHostname";
        private const string Ip = "1.2.3.4";
        private const string HttpRequestMethod = "POST";
        private const string Path = "1.2.3.4/testRoute";
        private const string Original = "Nunit Framework";
        private const string ReqId = "1";
        private const long Bytes = 42;
        private const int StatusCode = 200;
        private const decimal ResponseTime = 1337;
        private const string Msg = "request_completed";
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
                $@"{{""level"":{(int) LogLevels.Info},""time"":{Time},""reqId"":""{ReqId}"",""http"":{{""request"":{{""method"":""{HttpRequestMethod}"",""userAgent"":{{""original"":""{Original}""}}}},""response"":{{""statusCode"":{StatusCode},""body"":{{""bytes"":{Bytes}}}}}}},""url"":{{""path"":""{Path}""}},""host"":{{""hostname"":""{Hostname}"",""forwardedHostname"":""{ForwardedHostname}"",""ip"":""{Ip}""}},""responseTime"":{ResponseTime}.0,""msg"":""{Msg}""}}";
            Logger.LogRequest(_mockCompletedRequest);
            _mockILog.Verify(mock => mock.Info(expectedLog), Times.Once());
            
        }

        [Test]
        public void TestDebugLog()
        {
            var requestMocker = new HttpRequestTests();
            var mockRequest = requestMocker.CreateMockRequest();
            var logBeginsWith = $@"{{""level"":{(int) LogLevels.Debug},""time"":";
            var logEndsWith = 
                $@",""msg"":""message"",""reqId"":""{ReqId}"",""customPropA"":""foo"",""customPropB"":""bar""}}";
            Logger.Debug(mockRequest.Object, 
                "message", new CustomProps("foo", "bar"));
            _mockILog.Verify(mock => mock.Debug(It.Is<string>(str =>
                str.StartsWith(logBeginsWith) && str.EndsWith(logEndsWith))), Times.Once);
        }

        [Test]
        public void TestDebugLogWithRequestIdInDictionary()
        {
            var requestMocker = new HttpRequestTests();
            var dictionary = new Dictionary<object,object> 
            {
                { RequestResponseLoggingMiddleware.RequestIdDictionaryKey, "42"}
            };
            var mockRequest = requestMocker.CreateMockRequest(dictionary);
            var logBeginsWith = $@"{{""level"":{(int) LogLevels.Debug},""time"":";
            var logEndsWith = 
                $@",""msg"":""message"",""reqId"":""42"",""customPropA"":""foo"",""customPropB"":""bar""}}";
            Logger.Debug(mockRequest.Object, 
                "message", new CustomProps("foo", "bar"));
            _mockILog.Verify(mock => mock.Debug(It.Is<string>(str =>
                str.StartsWith(logBeginsWith) && str.EndsWith(logEndsWith))), Times.Once);
        }

        [Test]
        public void TestDebugLogWithoutRequest()
        {
            var logBeginsWith = $@"{{""level"":{(int) LogLevels.Debug},""time"":";
            var logEndsWith = 
                $@",""msg"":""message"",""customPropA"":""foo"",""customPropB"":""bar""}}";
            Logger.Debug("message", new CustomProps("foo", "bar"));
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
                $@",""msg"":""message"",""reqId"":""{ReqId}""}}";
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
                    Request = new ServiceRequest
                    {
                        Method = HttpRequestMethod,
                        UserAgent = new UserAgent
                        {
                            Original = Original
                        },
                    },
                    Response = new ServiceResponse
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
                    ForwardedHostname = ForwardedHostname,
                    Ip = Ip
                },
                ResponseTime = ResponseTime,
                Msg = Msg
            };
        }
    }
}
