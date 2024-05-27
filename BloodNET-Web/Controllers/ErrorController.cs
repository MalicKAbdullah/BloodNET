using Microsoft.AspNetCore.Mvc;

namespace BloodNET_Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("~/404.cshtml");
        }
    }
}
