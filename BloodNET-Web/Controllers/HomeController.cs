using BloodNET_Web.Models;
using BloodNET_Web.Models.Interfaces;
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
        private readonly IContacts _contactsRepository;
        private readonly IBloodRequests _bloodRequestsRepository;
        private readonly IDonation _donationRepository;

        private void sanitize(Contacts contact)
        {
            contact.Subject = System.Web.HttpUtility.HtmlEncode(contact.Subject);
            contact.Email = System.Web.HttpUtility.HtmlEncode(contact.Email);
            contact.Body = System.Web.HttpUtility.HtmlEncode(contact.Body);
            contact.PhoneNumber = System.Web.HttpUtility.HtmlEncode(contact.PhoneNumber);
            contact.Name = System.Web.HttpUtility.HtmlEncode(contact.Name);
        }

           

        public HomeController(ILogger<HomeController> logger, IContacts contactRepository, IBloodRequests bloodRequestsRepository, IDonation donationRepository)
        {
            _logger = logger;
            _contactsRepository = contactRepository;
            _bloodRequestsRepository = bloodRequestsRepository;
            _donationRepository = donationRepository;
       
        }

        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                TempData["userId"] = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                TempData["name"] = UsersRepository.GetNameandEmail(User.FindFirst(ClaimTypes.NameIdentifier)?.Value).name;
                TempData["email"] = UsersRepository.GetNameandEmail(User.FindFirst(ClaimTypes.NameIdentifier)?.Value).email;
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Contact(Contacts contacts)
        {

            if(!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is not valid");
                return View();
            }

            sanitize(contacts);

            
            _contactsRepository.Add(contacts);

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
            List<BloodRequests> bloodRequests = _bloodRequestsRepository.GetAllExcept(User.Identity.GetUserId());

            //if (HttpContext.Session.GetInt32("ReqId").HasValue)
            //{
            //    var reqId = HttpContext.Session.GetInt32("ReqId").Value;
            //    var itemToRemove = bloodRequests.SingleOrDefault(r => r.Id == reqId);
            //    if (itemToRemove != null)
            //        bloodRequests.Remove(itemToRemove);
            //}

            List<(string,int)> obj = _donationRepository.GetDonations(User.Identity.GetUserId());

            _bloodRequestsRepository.availibleRequests(bloodRequests, obj);
            return View(bloodRequests);
        }

        [Authorize]
      
        public IActionResult RealtimeRequests(string type)
        {
            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            //if (HttpContext.Session.GetInt32("ReqId").HasValue)
            //{
            //    bloodRequests = Delete(bloodRequests, HttpContext.Session.GetInt32("ReqId").Value);
            //}


            ViewBag.SelectedBloodType = type;

            if (type == "All")
            {
                bloodRequests = _bloodRequestsRepository.GetAllExcept(User.Identity.GetUserId());
            }
            else
            {
                bloodRequests = _bloodRequestsRepository.SearchByType(type,User.Identity.GetUserId());
            }

            return PartialView("_RealtimeRequests", bloodRequests);
            //return View(bloodRequests);

        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
