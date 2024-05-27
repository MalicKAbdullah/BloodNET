using BloodNET_Web.Models;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloodNET_Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
          
            BloodDonors bloodDonor = BloodDonorsRepository.GetDonorById(userId);

            ViewBag.CompleteProfile = bloodDonor != null;   

            return View(bloodDonor);
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
