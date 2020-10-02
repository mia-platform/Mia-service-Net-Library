using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MiaServiceDotNetLibrary.Decorators
{
    public class DecoratorResponse : IToActionResult
    {
        public DecoratorResponse(int statusCode, IDictionary<string, string> headers, ExpandoObject body)
        {
            StatusCode = statusCode;
            Headers = headers;
            Body = body;
        }

        public DecoratorResponse()
        {
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

        public virtual ActionResult ToActionResult(HttpContext context)
        {
            AddResponseHeaders(context);
            return new ContentResult
            {
                StatusCode = StatusCode,
                Content = JsonConvert.SerializeObject(Body)
            };
        }
        public ExpandoObject ToExpandoObject()
        {
            dynamic result = new ExpandoObject();
            result.statusCode = StatusCode;
            result.headers = Headers;
            result.body = Body;
            return result;
        }
    }
}
