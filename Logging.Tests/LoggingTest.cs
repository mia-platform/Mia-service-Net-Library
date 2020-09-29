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
                $@"{{""Level"":{Level},""Time"":{Time},""ReqId"":{ReqId},""Http"":{{""Request"":{{""Method"":""{HttpRequestMethod}"",""UserAgent"":{{""Original"":""{Original}""}}}},""Response"":{{""StatusCode"":{StatusCode},""Body"":{{""Bytes"":{Bytes}}}}}}},""Url"":{{""Path"":""{Path}""}},""Host"":{{""Hostname"":""{Hostname}"",""Ip"":""{Ip}""}},""ResponseTime"":{ResponseTime}.0}}";
            var mockRequest = BuildCompletedRequestLog();

            var appender = new log4net.Appender.MemoryAppender();
            BasicConfigurator.Configure(appender);
            LoggingUtility.LogRequest(mockRequest);
            var result = appender.GetEvents();
            Console.WriteLine(result);
            Console.WriteLine(result.Length);
            Console.WriteLine(result.Any());
            Console.WriteLine(result[0].MessageObject);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result[0].MessageObject.Equals(expectedLog));
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
