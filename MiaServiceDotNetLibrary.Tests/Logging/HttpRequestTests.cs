using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;

namespace MiaServiceDotNetLibrary.Tests.Logging
{
    public class MockBody
    {
        private string _message;

        public MockBody(string message)
        {
            _message = message;
        }
    }
    
    public class HttpRequestTests
    {
        private MemoryStream _memoryStream;
        
        public Mock<HttpRequest> CreateMockRequest()
        {
            var headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "x-request-id", "1"}
            }) as IHeaderDictionary;

            var json = JsonConvert.SerializeObject(new MockBody("Hello, World!"));
            var byteArray = Encoding.ASCII.GetBytes(json);
 
            _memoryStream = new MemoryStream(byteArray);
            _memoryStream.Flush();
            _memoryStream.Position = 0;
 
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(_memoryStream);
            mockRequest.Setup(x => x.Headers).Returns(headers);
            return mockRequest;
        }
    }
}
