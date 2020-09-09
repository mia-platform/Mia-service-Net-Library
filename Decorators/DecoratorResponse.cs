using System.Collections.Generic;
using System.Dynamic;

namespace Decorators
{
    public class DecoratorResponse
    {
        public DecoratorResponse(int statusCode, IDictionary<string, string> headers, object body)
        {
            StatusCode = statusCode;
            Headers = headers;
            Body = body;
        }

        public int StatusCode { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public object Body { get; set; }
    }
}
