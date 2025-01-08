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
        public async Task<IActionResult> Homework()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Get all of the homeworks of this specific user
            var homeworks = await _context.Homeworks.
                Where(s => s.StudentId == user.Id).
                ToListAsync();

            return View(homeworks);
        }

        [HttpGet]
        public async Task<IActionResult> EditHomework(string id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
                return NotFound();

            return View(homework);
        }

        // TODO: Create a homework viewmodel for this
        [HttpPost]
        public async Task<IActionResult> EditHomework(Homework homework)
        {
            if(ModelState.IsValid)
            {
                var updatedHomework = await _context.Homeworks.FindAsync(homework.Id);
                if(updatedHomework == null)
                    return NotFound();

                updatedHomework.Content = homework.Content;
                _context.Update(updatedHomework);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "A aparut o eroare, schimbarile nu au avut efect.");
                }
                return RedirectToAction(nameof(Index));
            }

            return View(homework);
        }
    }
}
