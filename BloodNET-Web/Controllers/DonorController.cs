using BloodNET_Web.Models;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using BloodNET_Web.Views.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Globalization;
using System.Security.Claims;

namespace BloodNET_Web.Controllers
{
    [Authorize]
    public class DonorController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";


        public DonorController(IWebHostEnvironment env)
        {
            _env = env;
        }

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

        public  int Eligibility(string  userId)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string selectQuery = "SELECT * FROM BloodDonors where Donorid = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", userId);

            int check = 0;

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            { 
                check = int.Parse(sqlDataReader["ismedicalfit"].ToString());
            }

            sqlConnection.Close();
            return check;
        }

        public IActionResult AddDonor()
        {
            return View();
        }

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

            var user = MyUsers.GetNameandEmail(userId);
            bloodDonors.Email = user.email;
            bloodDonors.Name = user.name;

            IRepository<BloodDonors> rep = new GenericRepository<BloodDonors>(connectionString);
            rep.Add(bloodDonors);
            return RedirectToAction("Index", "User");
        }
    }
}
