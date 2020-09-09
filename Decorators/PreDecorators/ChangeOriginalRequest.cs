using Decorators.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Decorators.PreDecorators
{
    public class ChangeOriginalRequest : DecoratorResponse
    {
        public ChangeOriginalRequest(DecoratorRequest body) : base(DecoratorConstants.ChangeOriginalStatusCode, DecoratorConstants.DefaultHeaders, body)
        {
        }
        
        public override ActionResult ToActionResult(HttpContext context)
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
