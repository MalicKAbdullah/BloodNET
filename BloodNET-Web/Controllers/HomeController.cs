using BloodNET_Web.Models;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public IActionResult Realtime()
        {
            BloodRequestsRepository bloodRequestsRepository = new BloodRequestsRepository();
            List<BloodRequests> bloodRequests = bloodRequestsRepository.GetAll();

            return View(bloodRequests);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
