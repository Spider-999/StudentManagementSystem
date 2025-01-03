using System.Diagnostics;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get the current user and their role/roles
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return View();

            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole == null)
                return View();

            // Get the first role of the user and redirect them to the appropriate page
            switch (userRole.First())
            {
                // TODO: Move this to AccountController
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                case "Teacher":
                    return RedirectToAction("Index", "Teacher");
                case "Student":
                    return RedirectToAction("Dashboard", "Student");
                default:
                    return View();
            }
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
