using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles ="Professor,Admin")]
    public class ProfessorController : Controller
    {
        #region Private properties
        private UserManager<User> _userManager;
        private AppDatabaseContext _context;
        #endregion

        #region Constructor
        public ProfessorController(UserManager<User> userManager, AppDatabaseContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            // Return 404 not found if the user wasnt found
            if (user == null)
                return NotFound();
            return View(user);
        }
    }
}
