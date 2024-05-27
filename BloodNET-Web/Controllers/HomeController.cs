using BloodNET_Web.Models;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace BloodNET_Web.Controllers
{ 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["userId"] = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                TempData["name"] = MyUsers.GetNameandEmail(User.FindFirst(ClaimTypes.NameIdentifier)?.Value).name;
                TempData["email"] = MyUsers.GetNameandEmail(User.FindFirst(ClaimTypes.NameIdentifier)?.Value).email;
            }
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

		public IActionResult Contact()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Contact(Contacts contacts)
        {
            ContactsRepository contactsRepository = new ContactsRepository();
            contactsRepository.Add(contacts);

            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public List<BloodRequests> Delete(List<BloodRequests> bloodRequests,int reqId)
        {
            List<BloodRequests> Requests = new List<BloodRequests>();
            foreach (var request in bloodRequests)
            {
                if (request.Id != reqId)
                    bloodRequests.Add(request);
            }

            return Requests;
        }

        [Authorize]
        public IActionResult Realtime()
        {
            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();
            List<BloodRequests> bloodRequests = bloodRequestsRepository.GetAll();

            //if (HttpContext.Session.GetInt32("ReqId").HasValue)
            //{
            //    var reqId = HttpContext.Session.GetInt32("ReqId").Value;
            //    var itemToRemove = bloodRequests.SingleOrDefault(r => r.Id == reqId);
            //    if (itemToRemove != null)
            //        bloodRequests.Remove(itemToRemove);
            //}

            DonationRepository donationRepository = new DonationRepository();
            List<(string,int)> obj = donationRepository.GetDonations(User.Identity.GetUserId());

            bloodRequestsRepository.availibleRequests(bloodRequests, obj);
            return View(bloodRequests);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Realtime(string type)
        {
            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();
            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            //if (HttpContext.Session.GetInt32("ReqId").HasValue)
            //{
            //    bloodRequests = Delete(bloodRequests, HttpContext.Session.GetInt32("ReqId").Value);
            //}


            ViewBag.SelectedBloodType = type;

            if (type == "All")
            {
                bloodRequests = bloodRequestsRepository.GetAll();
            }
            else
            {
                bloodRequests = bloodRequestsRepository.SearchByType(type);
            }
            return View(bloodRequests);

        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
