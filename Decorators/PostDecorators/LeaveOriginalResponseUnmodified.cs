using System.Collections.Generic;
using System.Dynamic;
using Decorators.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Decorators.PostDecorators
{
    public class LeaveOriginalResponseUnmodified : DecoratorResponse
    {
        public LeaveOriginalResponseUnmodified()
            : base(DecoratorConstants.LeaveOriginalUnchangedStatusCode, DecoratorConstants.DefaultHeaders, null)
        {
        }

        public override ActionResult ToActionResult(HttpContext context)
        {
            AddResponseHeaders(context);
            return new NoContentResult();
        }
    }
}
