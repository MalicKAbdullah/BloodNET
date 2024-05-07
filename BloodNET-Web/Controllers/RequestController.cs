using Microsoft.AspNetCore.Mvc;

namespace BloodNET_Web.Controllers
{
    public class RequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SubmitRequest()
        {
            return View();
        }
    }
}
