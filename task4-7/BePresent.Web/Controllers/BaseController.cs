using Microsoft.AspNetCore.Mvc;
namespace BePresent.Controllers
{
    public class BaseController : Controller
    {
        protected int? GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }
    }
}