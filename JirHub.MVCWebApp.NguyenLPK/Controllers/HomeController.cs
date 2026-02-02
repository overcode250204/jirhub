using JirHub.MVCWebApp.NguyenLPK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JirHub.MVCWebApp.NguyenLPK.Controllers
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
            HomeModel model = new HomeModel();
            return View(model);
        }

        public IActionResult GetCommitById(int commitId)
        {
            string commitString = commitId.ToString();
            return new ContentResult()
            {
                Content = commitString,
                ContentType = "text/plain"
            };
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
