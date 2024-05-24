using BloodNET_Web.Models;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloodNET_Web.Controllers
{
    public class DonationController : Controller
    {
        public readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Add([FromForm]int reqId)
        {
            int check = 0;
            if (HttpContext.Request.Cookies.ContainsKey("eligible"))
            {

                check = int.Parse(HttpContext.Request.Cookies["eligible"]);
            }
            else
            {
                CookieOptions option = new CookieOptions();
                //option.Expires = System.DateTime.Now.AddDays(1);
                DonorController donorController = new DonorController();
                check = donorController.Eligibility(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                //HttpContext.Response.Cookies.Append("first_request", System.DateTime.Now.ToString(), option);
                HttpContext.Response.Cookies.Append("eligible", check.ToString());
            }

            if(check==0)
            {
                return View("NotEligible", "Donation");
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
