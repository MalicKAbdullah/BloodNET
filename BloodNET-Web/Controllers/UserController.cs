using BloodNET_Web.Models;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloodNET_Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly IBloodDonors _bloodDonors;
        private readonly IDonation _donation;
        private readonly IAdmin _admin;

        public UserController(IBloodDonors bloodDonors, IDonation donation)
        {
            _bloodDonors = bloodDonors;
            _donation = donation;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
   

            ViewBag.CompleteProfile = _bloodDonors.GetDonorById(userId) != null;

            ViewBag.donorStatus = _donation.getDonorStatus(userId);

            return View(_bloodDonors.GetDonorById(userId));
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

        public IActionResult delete(string id)
        {

            _admin.DeleteUser(id);
            return RedirectToAction("Users", "Admin");
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
