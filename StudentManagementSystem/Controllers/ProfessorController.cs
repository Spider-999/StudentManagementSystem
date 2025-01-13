using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;
using System.IO.Compression;

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

            // Try to find all of the homeworks for the professors disciplines.
            // Check if the homework is a template, I dont want to show the same 
            // copy of a homework because there might be hundreds of the same
            // homeworks for students.
            var homeworks = await _context.Homeworks
                                  .Where(h => h.DisciplineId == professor.DisciplineId && h.IsTemplate == true)
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


                try
                {
                    // Add a homework template so I dont show the same copies of the
                    // homework on the homeworks page of the professor.
                    Homework homeworkTemplate = new Homework
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
                        IsTemplate = true
                    };
                    _context.Homeworks.Add(homeworkTemplate);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    ModelState.AddModelError(string.Empty, "Nu a putut fi adaugat templateul pentru tema");
                    return View(model);
                }

                // Add the homework for every student in the corresponding discipline
                List<Homework> homeworks = CreateHomeworkType(model, professor, students);

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

        public List<Homework> CreateHomeworkType(HomeworkViewModel model, Professor professor, List<Student> students)
        {
            List<Homework> homeworks = new List<Homework>();
            switch (model.HomeworkType)
            {
                case "Assignment":
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
                            StudentId = student.Id,
                            IsTemplate = false
                        };
                        homeworks.Add(homework);
                    }
                    break;
                case "Project":
                    foreach (var student in students)
                    {
                        Homework homework = new Project
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
                            StudentId = student.Id,
                            IsTemplate = false
                        };
                        homeworks.Add(homework);
                    }
                    break;
            }
            return homeworks;
        }

        [HttpGet]
        public async Task<IActionResult> EditHomework(string homeworkId)
        {
            var homework = await _context.Homeworks.FindAsync(homeworkId);
            if(homework == null)
                return NotFound();

            var model = new HomeworkViewModel
            {
                Id = homework.Id,
                Title = homework.Title,
                Description = homework.Description,
                EndDate = homework.EndDate,
                Mandatory = homework.Mandatory,
                Penalty = homework.Penalty,
                AfterEndUploadDate = homework.AfterEndDateUpload
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditHomework(HomeworkViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Query the professors homework template
                var updatedHomeworkTemplate = await _context.Homeworks.FindAsync(model.Id);
                if (updatedHomeworkTemplate == null)
                    return NotFound();

                // Query all of the students copies of the homework template that match the title of the homework template
                var studentHomeworks = await _context.Homeworks
                                            .Where(h => h.DisciplineId == updatedHomeworkTemplate.DisciplineId
                                                                       && h.IsTemplate == false
                                                                       && h.Title == updatedHomeworkTemplate.Title)
                                            .ToListAsync();

                // Update the homework template of the professor
                updatedHomeworkTemplate.Title = model.Title;
                updatedHomeworkTemplate.Description = model.Description;
                updatedHomeworkTemplate.EndDate = model.EndDate;
                updatedHomeworkTemplate.Mandatory = model.Mandatory;
                updatedHomeworkTemplate.Penalty = model.Penalty;
                updatedHomeworkTemplate.AfterEndDateUpload = model.AfterEndUploadDate;
                _context.Update(updatedHomeworkTemplate);

                // Update the homeworks for all of the students
                foreach(var homework in studentHomeworks)
                {
                    homework.Title = model.Title;
                    homework.Description = model.Description;
                    homework.EndDate = model.EndDate;
                    homework.Mandatory = model.Mandatory;
                    homework.Penalty = model.Penalty;
                    homework.AfterEndDateUpload = model.AfterEndUploadDate;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "A aparut o eroare, schimbarile nu au avut efect.");
                }
                return RedirectToAction(nameof(Homeworks));
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteHomework(string homeworkId)
        {
            // Find the homework template by its ID
            var homeworkTemplate = await _context.Homeworks.FindAsync(homeworkId);
            if (homeworkTemplate == null)
                return NotFound();

            // Find all corresponding student homeworks that match the template
            var studentHomeworks = await _context.Homeworks
                .Where(h => h.DisciplineId == homeworkTemplate.DisciplineId
                            && h.IsTemplate == false
                            && h.Title == homeworkTemplate.Title)
                .ToListAsync();

            // Delete the homework template
            _context.Homeworks.Remove(homeworkTemplate);

            // Delete the corresponding student homeworks
            _context.Homeworks.RemoveRange(studentHomeworks);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "A aparut o eroare, tema nu a putut fi stearsa.");
            }

            return RedirectToAction(nameof(Homeworks));
        }

        [HttpGet]
        public async Task<IActionResult> DownloadProjectFiles(string projectId)
        {
            var projectFiles = await _context.ProjectFiles
                                    .Where(p => p.ProjectID == projectId)
                                    .ToListAsync();
            if(projectFiles == null)
                return NotFound();

            // To read and write the byte array data of the file conten
            using (var memoryStream = new MemoryStream())
            {
                // Use zip archive to download all of the project files for the professor
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create))
                {
                    foreach(var file in projectFiles)
                    {
                        // Create the zip archive with the name of the file
                        var zip = zipArchive.CreateEntry(file.FileName, CompressionLevel.Optimal);
                        using(var zipStream = zip.Open())
                        {
                            await zipStream.WriteAsync(file.FileContent, 0, file.FileContent.Length);
                        }
                    }
                }
                // Create a random name for the zip file
                var zipName = Path.GetTempFileName() + ".zip";
                // Return the file with the specified MIME(indicates the type of a document)
                // type of application/zip
                return File(memoryStream.ToArray(),"applcation/zip", zipName);
            }
        }
    }
}
