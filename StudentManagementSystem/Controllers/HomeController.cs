using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
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
            // This logic is for when the user closes the site
            // and opens it again to redirect them to their specific role pages
            // instead of seeing the Home controllers basic index page.
            if(User.Identity.IsAuthenticated)
            {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return View();

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (await _userManager.IsInRoleAsync(user, "Student"))
                {
                    return RedirectToAction("Index", "Student");
                }
                else if (await _userManager.IsInRoleAsync(user, "Professor"))
                {
                    return RedirectToAction("Index", "Professor");
                }
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
