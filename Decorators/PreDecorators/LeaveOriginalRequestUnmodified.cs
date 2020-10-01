using Decorators.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Decorators.PreDecorators
{
    public class LeaveOriginalRequestUnmodified : DecoratorResponse
    {
        public LeaveOriginalRequestUnmodified() : base(DecoratorConstants.LeaveOriginalUnchangedStatusCode, DecoratorConstants.DefaultHeaders, null)
        {
        }

        public override ActionResult ToActionResult(HttpContext context)
        {
            AddResponseHeaders(context);
            return new NoContentResult();
        }
    }
}
