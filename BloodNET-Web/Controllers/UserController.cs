using BloodNET_Web.Models;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloodNET_Web.Controllers
{
    
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            BloodDonorsRepository bloodDonorsRepository = new BloodDonorsRepository();
            BloodDonors bloodDonor = bloodDonorsRepository.GetDonorById(userId);

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
