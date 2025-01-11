using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> ViewStudents()
        {
            // Get all of the students from the database and put them in a list
            var students = await _context.Students.ToListAsync();

            if(students == null)
                return NotFound();

            // Pass the student list to the view
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> ViewStudentHomeworks(string studentId)
        {
            // Get the student the professor clicked on
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return NotFound();

            // Get the student's homeworks
            var homeworks = await _context.Homeworks
                .Where(s => s.StudentId == studentId)
                .ToListAsync();

            ViewBag.StudentName = student.Name;
            return View(homeworks);
        }
    }
}
