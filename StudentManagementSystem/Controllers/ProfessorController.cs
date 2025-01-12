using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Professor,Admin")]
    public class ProfessorController : Controller
    {
        #region Private properties
        private UserManager<User> _userManager;
        private AppDatabaseContext _context;
        private readonly ILogger<ProfessorController> _logger;
        #endregion

        #region Constructor
        public ProfessorController(UserManager<User> userManager, 
                                   AppDatabaseContext context, 
                                   ILogger<ProfessorController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
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

            if (students == null)
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

        [HttpGet]
        public async Task<IActionResult> Homeworks()
        {
            // TODO: Add some custom exceptions for when a user is not found
            // a professor is not found and homeworks are not found.
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();


            // Try to find the professor
            var professor = await _context.Professors
                                  .Include(p => p.Discipline)
                                  .FirstOrDefaultAsync(p => p.Id == user.Id);
            if (professor == null)
                return NotFound();

            // Try to find all of the homeworks for the professors disciplines
            var homeworks = await _context.Homeworks
                                  .Where(h => h.DisciplineId == professor.DisciplineId)
                                  .ToListAsync();

            ViewBag.DisciplineName = professor.Discipline.Name;
            return View(homeworks);
        }

        [HttpGet]
        public IActionResult CreateHomework()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHomework(HomeworkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return NotFound();

                var professor = await _context.Professors
                                      .Include(p => p.Discipline)
                                      .FirstOrDefaultAsync(p => p.Id == user.Id);
                if (professor == null)
                    return NotFound();

                var students = await _context.Students
                                      .Where(s => s.StudentDisciplines.Any(sd => sd.DisciplineId == professor.DisciplineId))
                                      .ToListAsync();
                if (students == null)
                    return NotFound();

                // Add the homework for every student
                List<Homework> homeworks = new List<Homework>();
                foreach (var student in students)
                {
                    Homework homework = new Homework
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Content = string.Empty,
                        Grade = 0.00,
                        Status = false,
                        Mandatory = model.Mandatory,
                        Penalty = model.Penalty,
                        AfterEndDateUpload = model.AfterEndUploadDate,
                        DisciplineId = professor.DisciplineId,
                        StudentId = student.Id
                    };
                    homeworks.Add(homework);
                }

                try
                {
                    _context.Homeworks.AddRange(homeworks);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ModelState.AddModelError("", "A aparut o eroare, nu am putut creea tema.");
                    return View(model);
                }
                
                return RedirectToAction(nameof(Homeworks));
            }

            return View(model);
        }
    }
}
