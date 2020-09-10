using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Decorators.PostDecorators
{
    public class LeaveOriginalResponseUnmodified : DecoratorResponse, IToActionResult
    {
        public LeaveOriginalResponseUnmodified(int statusCode, IDictionary<string, string> headers, ExpandoObject body)
            : base(statusCode, headers, body)
        {
        }

        public ActionResult ToActionResult(HttpContext context)
        {
            AddResponseHeaders(context);
            return new NoContentResult();
        }
    }
}
