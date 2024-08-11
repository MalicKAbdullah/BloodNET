using BloodNET_Web.Models;
using BloodNET_Web.Models.Hubs;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;

namespace BloodNET_Web.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class RequestController : Controller
    {

        private readonly IBloodDonors _bloodDonors;
        private readonly IDonation _donation;

        private readonly IBloodRequests _bloodRequestsRepository;

        private readonly IRepository<BloodRequests> _bloodRequest;

        private readonly IHubContext<BloodRequestHub> _hubContext;
        public RequestController(IBloodDonors bloodDonors,IBloodRequests repository,IDonation donation, IRepository<BloodRequests> bloodRequest, IHubContext<BloodRequestHub> hubContext)
        {
            _bloodDonors = bloodDonors;
            _bloodRequestsRepository = repository;
            _donation = donation;
            _bloodRequest = bloodRequest;
            _hubContext = hubContext;
        }

        private void sanitize(BloodRequests bloodRequests)
        {
            bloodRequests.RecipientName = HttpUtility.HtmlEncode(bloodRequests.RecipientName);
            bloodRequests.RecipientPhone = HttpUtility.HtmlEncode(bloodRequests.RecipientPhone);
            bloodRequests.Location = HttpUtility.HtmlEncode((string)bloodRequests.Location);
            bloodRequests.Description = HttpUtility.HtmlEncode(bloodRequests.Description);
        }

        public IActionResult Index()
        {

            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            List<BloodRequests> bloodRequests = _bloodRequestsRepository.Get(userid);

            List<List<string>> donorIds = new List<List<string>>();
            List<DateTime> dateTimes = new List<DateTime>();

            foreach (var bloodRequest in bloodRequests)
            {
                List<(string,DateTime)> obj = new List<(string, DateTime)>();
                obj = _donation.GetDonors(bloodRequest.Id);
                List<string> ids = obj.Select(x => x.Item1).ToList();
                List<DateTime> created = obj.Select(x => x.Item2).ToList();
                dateTimes.AddRange(created);
                donorIds.Add(ids);
           }

            List<List<BloodDonors>> bloodDonors = new List<List<BloodDonors>>();
            foreach(var ids in donorIds)
            {
                List<BloodDonors> donors = _bloodDonors.GetDonorsById(ids);
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SubmitRequest(string bloodgroup, DateTime dtime, string rname, string rphone, string raddress, string description)
        {

            if(!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is not valid");
                return View(SubmitRequest());

            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            BloodRequests bloodRequests = new BloodRequests(bloodgroup, dtime, rname, rphone, raddress, description,userId);

            sanitize(bloodRequests);

            _bloodRequestsRepository.Add(bloodRequests);


            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveBloodRequest", bloodgroup, dtime, rname,raddress);
            }
            else
            {
                Debug.WriteLine("_hubContext is null");
            }



            return RedirectToAction("Index", "Request");

        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy ="AdminPolicy")]
        public IActionResult delete(int reqId)
        {
            _bloodRequest.Delete(reqId);
            return RedirectToAction("Requests", "Admin");
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

        [HttpPost]

        public IActionResult CloseRequest(int reqId)
        {
            _bloodRequestsRepository.UpdateStatus(reqId, "False");

            return RedirectToAction("Index","Request");
        }


    }
}
