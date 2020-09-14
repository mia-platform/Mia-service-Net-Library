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
        
        [Test]
        public void TestIncomingRequestLog()
        {
            const string expectedLog =
                "{\"Http\":{\"Request\":{\"Method\":\"" + HttpRequestMethod + "\"},\"Response\":null}," +
                "\"Url\":" +
                    "{\"Path\":\"" + Path + "\"}," +
                    "\"UserAgent\":" +
                        "{\"Original\":\"" + Original + "\"}," +
                    "\"Host\":" +
                        "{\"Hostname\":\"" + Hostname + "\",\"Ip\":\"" + Ip + "\"}}";
            var mockRequest = BuildIncomingRequestLog();
            var appender = new log4net.Appender.MemoryAppender();
            BasicConfigurator.Configure(appender);
            LoggingUtility.LogIncomingRequest(mockRequest);
            
            var result = appender.GetEvents();
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result[0].MessageObject.Equals(expectedLog));
        }
        
        [Test]
        public void TestCompletedRequestLog()
        {
            var expectedLog =
                "{\"Http\":{\"Request\":{\"Method\":\"" + HttpRequestMethod + "\"}," +
                "\"Response\":{\"StatusCode\":" + StatusCode + ",\"Body\":{\"Bytes\":"+ Bytes + "}}}," +
                "\"Url\":" +
                    "{\"Path\":\"" + Path + "\"}," +
                    "\"UserAgent\":" +
                        "{\"Original\":\"" + Original + "\"}," +
                    "\"Host\":" +
                        "{\"Hostname\":\"" + Hostname + "\",\"Ip\":\"" + Ip + "\"}," +
                "\"ResponseTime\":0.0}";
            var mockRequest = BuildCompletedRequestLog();
            
            var appender = new log4net.Appender.MemoryAppender();
            BasicConfigurator.Configure(appender);
            LoggingUtility.LogCompletedRequest(mockRequest);
            
            var result = appender.GetEvents();
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result[0].MessageObject.Equals(expectedLog));
        }

        private static CompletedRequestLog BuildCompletedRequestLog()
        {
            return new CompletedRequestLog
            {
                Http = new Http
                {
                    Request = new CpRequest
                    {
                        Method = HttpRequestMethod
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
                UserAgent = new UserAgent
                {
                    Original = Original
                },
                Host = new Host
                {
                    Hostname = Hostname,
                    Ip = Ip
                },
                ResponseTime = 0
            };
        }
        private static IncomingRequestLog BuildIncomingRequestLog()
        {
            return new IncomingRequestLog
            {
                Host = new Host
                {
                    Hostname = Hostname,
                    Ip = Ip
                },
                Http = new Http
                {
                    Request = new CpRequest
                    {
                        Method = HttpRequestMethod
                    }
                },
                Url = new Url
                {
                    Path = Path
                },
                UserAgent = new UserAgent
                {
                    Original = Original
                }
            };
        }
    }
}