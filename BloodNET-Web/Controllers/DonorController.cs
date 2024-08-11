using BloodNET_Web.Models;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using BloodNET_Web.Views.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Web;

namespace BloodNET_Web.Controllers
{
    [Authorize]
    public class DonorController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly IRepository<BloodDonors> _bloodDonorRepository;

        private void sanitize(BloodDonors bloodDonors)
        {
            bloodDonors.City = HttpUtility.HtmlEncode(bloodDonors.City);
            bloodDonors.Address = HttpUtility.HtmlEncode(bloodDonors.Address);
            bloodDonors.Country = HttpUtility.HtmlEncode((string)bloodDonors.Country);
            bloodDonors.MedicalHistory = HttpUtility.HtmlEncode(bloodDonors.MedicalHistory);
            bloodDonors.PhoneNumber = HttpUtility.HtmlEncode(bloodDonors.PhoneNumber);
        }

        public DonorController(IWebHostEnvironment env,IRepository<BloodDonors> _repo)
        {
            _env = env;
            _bloodDonorRepository = _repo;
        }
        //public DonorController(IRepository<BloodDonors> _repo)
        //{
        //    _bloodDonorRepository = _repo;
        //}

        public IActionResult Index()
        {
            return View();
        }
        public string getImageUrl(IFormFile picture)
        {
            string wwwrootPath = _env.WebRootPath;
            string path = Path.Combine(wwwrootPath, "User", "Profile");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string UFileName = Guid.NewGuid().ToString() + "-" + picture.FileName;
            if (picture != null && picture.Length > 0)
            {
                path = Path.Combine(path, UFileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    picture.CopyTo(fileStream);
                }
            }
            return Path.Combine("User", "Profile", UFileName);
        }


        public IActionResult AddDonor()
        {
            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Add(BloodDonors bloodDonors)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bloodDonors.DonorId = userId;
            bloodDonors.ImgUrl = getImageUrl(bloodDonors.Image);

            if (bloodDonors.MedicalHistory.IsNullOrEmpty())
            {
                bloodDonors.MedicalHistory = "NULL";
            }

            var user = UsersRepository.GetNameandEmail(userId);
            bloodDonors.Email = user.email;
            bloodDonors.Name = user.name;

            //if (!ModelState.IsValid)
            //{
            //    Debug.WriteLine("Model State is not valid");
            //    return View("AddDonor");
            //}

            sanitize(bloodDonors);

            _bloodDonorRepository.Add(bloodDonors);
            return RedirectToAction("Index", "User");
        }
    }
}
