using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Decorators
{
    public interface IToActionResult
    {
        public  ActionResult ToActionResult(HttpContext context);
    }
}
