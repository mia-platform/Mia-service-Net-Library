using System.Collections.Generic;
using System.Dynamic;
using MiaServiceDotNetLibrary.Decorators.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MiaServiceDotNetLibrary.Decorators
{
    public class AbortChainResponse : DecoratorResponse
    {
        public AbortChainResponse(int finalStatusCode, IDictionary<string, string> finalHeaders, ExpandoObject finalBody) : base(
            finalStatusCode, finalHeaders, finalBody)
        {
        }

        public override ActionResult ToActionResult(HttpContext context)
        {
            AddResponseHeaders(context);
            return new ContentResult
            {
                StatusCode = DecoratorConstants.AbortChainStatusCode,
                Content = JsonConvert.SerializeObject(this)
            }; 
        }
    }
}
