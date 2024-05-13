using BloodNET_Web.Models;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BloodNET_Web.Controllers
{
    public class RequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult SubmitRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitRequest(string bloodgroup, DateTime dtime, string rname, string rphone, string raddress, string description)
        {
            BloodRequests bloodRequests = new BloodRequests(bloodgroup, dtime, rname, rphone, raddress, description);

            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();

            bloodRequestsRepository.Add(bloodRequests);

            return RedirectToAction("Index", "Home");
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
