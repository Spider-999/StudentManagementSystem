using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;
using System.Data;
using System.IO.Compression;
using System.Text;
namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        #region Private properties
        private UserManager<User> _userManager;
        private AppDatabaseContext _context;
        private readonly ILogger<ProfessorController> _logger;
        #endregion

        #region Constructor 
        public AdminController(UserManager<User> userManager,
                           AppDatabaseContext context,
                           ILogger<ProfessorController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Admin methods
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            // Return 404 not found if the user wasnt found
            if (user == null)
                return NotFound();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserAndRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                // I dont want the admin users to be viewable
                if (!roles.Contains("Admin"))
                {
                    userRolesViewModel.Add(new UserAndRolesViewModel
                    {
                        User = user,
                        Roles = roles
                    });
                }
            }
            return View(userRolesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            // Find the user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID: {userId} not found.");
                return NotFound();
            }

            // Try to delete it
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ViewAllUsers));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction(nameof(ViewAllUsers));
        }
        #endregion


    }
}
