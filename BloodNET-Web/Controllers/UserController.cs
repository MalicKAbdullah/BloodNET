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

            ViewBag.donorStatus = DonationRepository.getDonorStatus(userId);

            return View(bloodDonor);
        }

        //[HttpPost]

        //public void CloseRequest(int reqId, string donorId)
        //{
        //    BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();
        //    bloodRequestsRepository.UpdateStatus(reqId, "False");

        //    BloodDonorsRepository bloodDonorsRepository = new BloodDonorsRepository();
        //    bloodDonorsRepository.UpdateStatus(donorId, 0);

        //    DonationRepository donationRepository = new DonationRepository();
        //    int donationId = donationRepository.getDonationId(donorId, reqId);

        //    donationRepository.UpdateStatus(donationId, 1);
        //}

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
