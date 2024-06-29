using BloodNET_Web.Models.Repository;
using BloodNET_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;
using BloodNET_Web.Models.Interfaces;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace BloodNET_Web.Controllers
{
    [Authorize(Policy ="AdminPolicy")]
    public class AdminController : Controller
    {
        public readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Requests()
        {
            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();
            List<BloodRequests> bloodRequests = bloodRequestsRepository.GetAll();

            DonationRepository donationRepository = new DonationRepository();
            List<(string, int)> obj = donationRepository.GetDonations(User.Identity.GetUserId());

            bloodRequestsRepository.availibleRequests(bloodRequests, obj);
            return View(bloodRequests);
        }

 
        [HttpPost]
        public IActionResult Requests(string type)
        {
            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();
            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            ViewBag.SelectedBloodType = type;

            if (type == "All")
            {
                bloodRequests = bloodRequestsRepository.GetAll();
            }
            else
            {
                bloodRequests = bloodRequestsRepository.SearchByType(type, " ");
            }
            return View(bloodRequests);

        }

        public IActionResult Users()
        {
            AdminRepository adminRepository = new AdminRepository();
            return View(adminRepository.GetUsers());
        }

        public IActionResult Donations()
        {
            AdminRepository adminRepository = new AdminRepository();
            return View(adminRepository.GetDonations());
        }

        public IActionResult Contact()
        {
            IRepository<Contacts> repository = new GenericRepository<Contacts>(connectionString);
            return View(repository.GetAll());
        }


    }
}
