using Microsoft.AspNetCore.Mvc;

namespace BloodNET_Web.Controllers
{
    
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }
    }
}
