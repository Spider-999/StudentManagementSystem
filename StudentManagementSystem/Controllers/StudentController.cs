using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Student,Admin")]
    public class StudentController : Controller
    {
        #region Private properties
        private UserManager<User> _userManager;
        private AppDatabaseContext _context;
        #endregion

        #region Constructor
        public StudentController(UserManager<User> userManager, AppDatabaseContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return Unauthorized();
            }

            var homeworks = await _context.Homeworks.
                Where(s => s.StudentId == user.Id).
                ToListAsync();

            return View(homeworks);
        }   
    }
}
