using BloodNET_Web.Models;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace BloodNET_Web.Controllers
{
    public class DonationController : Controller
    {
        public readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";
        private readonly DonorController _donorController;
        public DonationController(IServiceProvider serviceProvider)
        {
            _donorController = serviceProvider.GetRequiredService<DonorController>();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Add([FromForm]int reqId)
        {
            

            BloodDonors donor = BloodDonorsRepository.GetDonorById(User.Identity.GetUserId()); // Implement this method to check profile completion
            bool ProfileComplete = donor != null;

            if (!ProfileComplete)
            {
                ViewBag.message = "Your Donor Profile is Incomplete";
                ViewBag.cta = "Complete your Profile";
                return View("NotEligible");
            }

            int check = 0;
            if (HttpContext.Request.Cookies.ContainsKey("eligible"))
            {
                check = int.Parse(HttpContext.Request.Cookies["eligible"]);
            }
            else
            {
                CookieOptions option = new CookieOptions();
                //option.Expires = System.DateTime.Now.AddDays(1);
                check = _donorController.Eligibility(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                //HttpContext.Response.Cookies.Append("first_request", System.DateTime.Now.ToString(), option);
                HttpContext.Response.Cookies.Append("eligible", check.ToString());
            }

            if(check==0)
            {
                ViewBag.message = "Your are Medically Unfit as per your Provided Details";
                ViewBag.cta = "Update Your Donor Profile";
                return View("NotEligible");
            }


            Donation donation = new Donation();
            donation.RequestId = reqId;
            donation.DonorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
   
            IRepository<Donation> DonationRepository = new GenericRepository<Donation>(connectionString);
            DonationRepository.Add(donation);

            return RedirectToAction("Index","Home");
        }
    }
}
