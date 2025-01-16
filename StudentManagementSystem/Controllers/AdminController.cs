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
        private UserManager<IdentityUser> _userManager;
        private AppDatabaseContext _context;
        private readonly ILogger<ProfessorController> _logger;
        #endregion
        #region Constructor 
        public AdminController(UserManager<IdentityUser> userManager,
                           AppDatabaseContext context,
                           ILogger<ProfessorController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        #endregion
        #region User methods
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            // Return 404 not found if the user wasnt found
            if (user == null)
                return NotFound();
            return View(user);
        }
        #endregion


    }
}
