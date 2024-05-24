using BloodNET_Web.Models.Repository;
using BloodNET_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodNET_Web.Controllers
{
    [Authorize(Policy ="AdminPolicy")]
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApproveRequests()
        {
            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();
            List<BloodRequests> bloodRequests = bloodRequestsRepository.GetAll();

            return View(bloodRequests);
        }

    }
}
