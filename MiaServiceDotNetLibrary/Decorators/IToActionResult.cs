using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiaServiceDotNetLibrary.Decorators
{
    public interface IToActionResult
    {
        public  ActionResult ToActionResult(HttpContext context);
    }
}
