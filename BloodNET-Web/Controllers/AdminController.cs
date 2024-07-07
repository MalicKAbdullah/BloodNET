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
        private readonly IAdmin _admin;
        private readonly IBloodRequests _bloodRequests;
        private readonly IDonation _donation;

        public AdminController(IAdmin admin, IBloodRequests bloodRequests, IDonation donation)
        {
            _admin = admin;
            _bloodRequests = bloodRequests;
            _donation = donation;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Requests()
        {
            List<BloodRequests> bloodRequests = _bloodRequests.GetAll();

    
            List<(string, int)> obj = _donation.GetDonations(User.Identity.GetUserId());

            _bloodRequests.availibleRequests(bloodRequests, obj);
            return View(bloodRequests);
        }

 
        [HttpPost]
        public IActionResult Requests(string type)
        {

            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            ViewBag.SelectedBloodType = type;

            if (type == "All")
            {
                bloodRequests = _bloodRequests.GetAll();
            }
            else
            {
                bloodRequests = _bloodRequests.SearchByType(type, " ");
            }
            return View(bloodRequests);

        }

        public IActionResult Users()
        {

            return View(_admin.GetUsers());
        }

        public IActionResult Donations()
        {

            return View(_admin.GetDonations());
        }

        public IActionResult Contact()
        {
            IRepository<Contacts> repository = new GenericRepository<Contacts>(connectionString);
            return View(repository.GetAll());
        }


    }
}
