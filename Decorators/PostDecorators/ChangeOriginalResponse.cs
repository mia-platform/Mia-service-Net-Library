using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Decorators.PostDecorators
{
    public class ChangeOriginalResponse : DecoratorResponse, IToActionResult
    {
        public ChangeOriginalResponse(int statusCode, IDictionary<string, string> headers, ExpandoObject body) : base(statusCode, headers, body)
        {
        }

        public ActionResult ToActionResult(HttpContext context)
        {
            AddResponseHeaders(context);
            return new ContentResult()
            {
                StatusCode = StatusCode,
                Content = JsonConvert.SerializeObject(Body)
            };
        }
    }
}
