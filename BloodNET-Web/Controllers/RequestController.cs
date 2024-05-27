using BloodNET_Web.Models;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloodNET_Web.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {

        public readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";

        public IActionResult Index()
        {
            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();

            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            List<BloodRequests> bloodRequests = bloodRequestsRepository.Get(userid);

            List<List<string>> donorIds = new List<List<string>>();
            List<DateTime> dateTimes = new List<DateTime>();

            DonationRepository donationRepository = new DonationRepository();
            foreach (var bloodRequest in bloodRequests)
            {
                List<(string,DateTime)> obj = new List<(string, DateTime)>();
                obj = donationRepository.GetDonors(bloodRequest.Id);
                List<string> ids = obj.Select(x => x.Item1).ToList();
                List<DateTime> created = obj.Select(x => x.Item2).ToList();
                dateTimes.AddRange(created);
                donorIds.Add(ids);
           }

            List<List<BloodDonors>> bloodDonors = new List<List<BloodDonors>>();
            foreach(var ids in donorIds)
            {
                List<BloodDonors> donors = BloodDonorsRepository.GetDonorsById(ids);
                bloodDonors.Add(donors);
            }

            Respondants respondants = new Respondants 
            { 
                BloodDonors = bloodDonors,
                BloodRequests = bloodRequests,
                DateTimes = dateTimes,
            };
            return View(respondants);
        }

        public ViewResult SubmitRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitRequest(string bloodgroup, DateTime dtime, string rname, string rphone, string raddress, string description)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            BloodRequests bloodRequests = new BloodRequests(bloodgroup, dtime, rname, rphone, raddress, description,userId);

            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();

            bloodRequestsRepository.Add(bloodRequests);

            return RedirectToAction("Index", "Request");

        }

        [HttpPost]

        public IActionResult delete(int reqId)
        {
            IRepository<BloodRequests> repository = new GenericRepository<BloodRequests>(connectionString);
            repository.Delete(reqId);
            return RedirectToAction("Index", "Request");
        }



        //[HttpPost]
        //public IActionResult SubmitRequestPost()
        //{
        //    string bgroup = Request.Form["bloodgroup"];
        //    DateTime dtime = DateTime.Parse(Request.Form["dtime"].ToString());
        //    string rname = Request.Form["rname"];
        //    string rphone = Request.Form["rphone"];
        //    string raddress = Request.Form["raddress"];
        //    string desc = Request.Form["description"];

        //    BloodRequests bloodRequests = new BloodRequests(bgroup, dtime, rname, rphone, raddress, desc);

        //    BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();
        //    bloodRequestsRepository.Add(bloodRequests);

        //    return RedirectToAction("index", "Home");
        //}


    }
}
