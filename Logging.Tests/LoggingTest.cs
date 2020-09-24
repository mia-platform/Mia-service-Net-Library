using System;
using System.Linq;
using log4net.Config;
using Logging.Entities;
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
                "{\"level\":" + Level + ",\"time\":" + Time + ",\"reqId\":"+ ReqId +",\"http\":{\"request\":{\"method\":\"" + HttpRequestMethod + "\"}," +
                "\"response\":{\"statusCode\":" + StatusCode + ",\"body\":{\"bytes\":"+ Bytes + "}}}," +
                "\"url\":" +
                    "{\"path\":\"" + Path + "\"}," +
                    "\"userAgent\":" +
                        "{\"original\":\"" + Original + "\"}," +
                    "\"host\":" +
                        "{\"hostname\":\"" + Hostname + "\",\"ip\":\"" + Ip + "\"}," +
                "\"responseTime\":" + ResponseTime + ".0}";
            var mockRequest = BuildCompletedRequestLog();
            
            var appender = new log4net.Appender.MemoryAppender();
            BasicConfigurator.Configure(appender);
            LoggingUtility.LogRequest(mockRequest);
            
            var result = appender.GetEvents();
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result[0].MessageObject.Equals(expectedLog));
        }

        private static RequestLog BuildCompletedRequestLog() =>
            new RequestLog
            {
                level = Level,
                time = Time,
                reqId = ReqId,
                http = new Http
                {
                    request = new CpRequest
                    {
                        method = HttpRequestMethod
                    },
                    response = new CpResponse
                    {
                        body = new Body
                        {
                            bytes = Bytes
                        },
                        statusCode = StatusCode
                    }
                },
                url = new Url 
                {
                    path = Path,
                },
                userAgent = new UserAgent
                {
                    original = Original
                },
                host = new Host
                {
                    hostname = Hostname,
                    ip = Ip
                },
                responseTime = ResponseTime
            };
    }
}
