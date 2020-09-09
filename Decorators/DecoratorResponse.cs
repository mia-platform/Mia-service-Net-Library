using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Decorators
{
    public abstract class DecoratorResponse
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

        protected void AddResponseHeaders(HttpContext context)
        {
            foreach (var (key, value) in Headers)
            {
                context.Response.Headers.Add(key, value);
            }
        }

        public abstract ActionResult ToActionResult(HttpContext context);
    }
}
