using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

namespace Decorators
{
    public class DecoratorResponse
    {
        public DecoratorResponse(int statusCode, IDictionary<string, string> headers, ExpandoObject body)
        {
            StatusCode = statusCode;
            Headers = headers;
            Body = body;
        }

        public int StatusCode { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public ExpandoObject Body { get; set; }

        protected void AddResponseHeaders(HttpContext context)
        {
            foreach (var (key, value) in Headers)
            {
                context.Response.Headers.Add(key, value);
            }
        }
    }
}
