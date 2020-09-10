using Decorators.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Decorators.PreDecorators
{
    public class ChangeOriginalRequest : DecoratorResponse, IToActionResult
    {
        public ChangeOriginalRequest(DecoratorRequest body) : base(DecoratorConstants.ChangeOriginalStatusCode, DecoratorConstants.DefaultHeaders, body.ToExpandoObject())
        {
        }
        
        public ActionResult ToActionResult(HttpContext context)
        {
            AddResponseHeaders(context);
            return new ContentResult
            {
                StatusCode = StatusCode,
                Content = JsonConvert.SerializeObject(Body)
            };
        }
    }
}
