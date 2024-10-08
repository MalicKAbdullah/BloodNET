﻿using BloodNET_Web.Models;
using BloodNET_Web.Models.Hubs;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace BloodNET_Web.Controllers
{
    public class DonationController : Controller
    {
        private readonly DonorController _donorController;
        private readonly IBloodDonors _bloodDonors;
        private readonly IRepository<Donation> _donationRepository;
        private readonly IHubContext<BloodDonationHub> _hubContext;

        public DonationController(IServiceProvider serviceProvider, IBloodDonors bloodDonors,IRepository<Donation> _donationRepo,IHubContext<BloodDonationHub> hubContext)
        {
            _donorController = serviceProvider.GetRequiredService<DonorController>();
            _bloodDonors = bloodDonors;
            _donationRepository = _donationRepo;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Add([FromForm]int reqId)
        {
            

            BloodDonors donor = _bloodDonors.GetDonorById(User.Identity.GetUserId()); // Implement this method to check profile completion
            bool ProfileComplete = donor != null;

            if (!ProfileComplete)
            {
                ViewBag.message = "Your Donor Profile is Incomplete";
                ViewBag.cta = "Complete your Profile";
                ViewBag.action = "AddDonor";
                ViewBag.controller = "Donor";
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
                check = _bloodDonors.Eligibility(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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
   

            _donationRepository.Add(donation);

            _hubContext.Clients.All.SendAsync("ReceiveDonation", UsersRepository.GetNameandEmail( donation.DonorId).name);

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult delete(int Id)
        {
            _donationRepository.Delete(Id);
            return RedirectToAction("Donations", "Admin");
        }
    }
}
