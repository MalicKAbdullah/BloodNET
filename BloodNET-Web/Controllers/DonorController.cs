﻿using Microsoft.AspNetCore.Mvc;

namespace BloodNET_Web.Controllers
{
    public class DonorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddDonor()
        {
            return View();
        }

    }
}