using Diplom.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Diplom.Controllers
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
            string? userId = HttpContext.Session.GetString("UserId");
            string? username = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(username))
            {
                ViewBag.Message = $"Привіт, {username}! Ваш ID: {userId}";
            }
            else
            {
                ViewBag.Message = "Ви не увійшли в систему.";
            }

            return View();
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
