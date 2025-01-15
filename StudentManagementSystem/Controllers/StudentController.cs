using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Student,Admin")]
    public class StudentController : Controller
    {
        #region Private properties
        private UserManager<User> _userManager;
        private AppDatabaseContext _context;
        private readonly ILogger<ProfessorController> _logger;
        #endregion

        #region Constructor
        public StudentController(UserManager<User> userManager, AppDatabaseContext context, ILogger<ProfessorController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    //Exception thrown if user is not found
                    throw new StudentControllerException("User not found", 404);
                }
                return View(user);
            }
            catch (StudentControllerException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Homework()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    //Exception thrown if user is null, meaning they have not authenticated
                    throw new StudentControllerException("User not authorised", 401);
                }
                // Get all of the homeworks of this specific user
                var homeworks = await _context.Homeworks.
                    Where(s => s.StudentId == user.Id).
                    ToListAsync();

                return View(homeworks);
            }

            catch (StudentControllerException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditHomework(string id)
        {
            try
            {
                var homework = await _context.Homeworks.FindAsync(id);
                if (homework == null)
                {
                    //Exception thrown if homework is not found
                    throw new StudentControllerException("Homework not found", 404);
                }

                var model = new HomeworkViewModel
                {
                    Id = homework.Id,
                    Title = homework.Title,
                    Content = homework.Content,
                    Description = homework.Description,
                    EndDate = homework.EndDate,
                    Mandatory = homework.Mandatory,
                    Penalty = homework.Penalty,
                    AfterEndUploadDate = homework.AfterEndDateUpload
                };
                return View(model);
            }
            catch (StudentControllerException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditHomework(HomeworkViewModel homework)
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
                return RedirectToAction(nameof(Homework));
            }

            return View(homework);
        }

        [HttpGet]
        public IActionResult UploadProjectFiles(string projectID)
        {
            // Project ID is needed for the post request upload.
            var model = new ProjectFileUploadViewModel
            {
                ProjectID = projectID
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProjectFiles(ProjectFileUploadViewModel model)
        {
            if (model.Files == null || model.Files.Count <= 0)
                return View(model);

            // TODO: Add try-catch statements
            if(ModelState.IsValid)
            {
                var project = await _context.Projects.FindAsync(model.ProjectID);
                if(project == null)
                    return NotFound();

                // Iterate through all of the models files sent by the user
                foreach(var file in model.Files)
                {
                    // To read and write the byte array data of the file content
                    using(var memoryStream = new MemoryStream())
                    {
                        // Copy file contents to the memory stream
                        await file.CopyToAsync(memoryStream);
                        // Create a project file for each file sent by the user
                        var projectFile = new ProjectFile
                        {
                            ProjectID = project.Id,
                            FileName = file.FileName,
                            FileContent = memoryStream.ToArray()
                        };
                        // Save that file in the database
                        _context.ProjectFiles.Add(projectFile);
                    }
                }

                // Set the status to true because the project files have been uploaded
                // and the project is now completed
                project.Status = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Homework));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CompleteQuiz(string id)
        {
            var quiz = await _context.Quizzes
                              .Include(q => q.QuizQuestions)
                              .FirstOrDefaultAsync(q => q.Id ==  id);

            if (quiz == null)
                return NotFound();

            var model = new CompleteQuizViewModel
            {
                QuizID = quiz.Id,
                Title = quiz.Title,
                TimeLimit = quiz.TimeLimit, 
                Questions = quiz.QuizQuestions.Select(q => new CompleteQuizQuestionViewModel
                {
                    Question = q.Question,
                    Answers = q.Answers,
                    SelectedAnswer = null
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteQuiz(CompleteQuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError($"Validation error in {state.Key}: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }

            var quiz = await _context.Quizzes
                             .Include(q => q.QuizQuestions)
                             .FirstOrDefaultAsync(q => q.Id == model.QuizID);

            if (quiz == null)
                return NotFound();

            // Get the correct answers
            int correctAnswers = 0;
            foreach(var question in model.Questions)
            {
                var quizQuestion = quiz.QuizQuestions.FirstOrDefault(q => q.Question == question.Question);

                if (quizQuestion == null)
                {
                    _logger.LogWarning($"Question not found: {question.Question}");
                    continue;
                }

                if (quizQuestion.CorrectAnswer == question.SelectedAnswer)
                {
                    
                    correctAnswers++;
                }
            }

            // If the homework was uploaded after the deadline substract the penalty from the final grade
            if (quiz.AfterEndDateUpload == true && DateTime.Now >= quiz.EndDate)
            {
                double grade = ((double)correctAnswers / model.Questions.Count) * 9.00 + 1.00 - (double)quiz.Penalty;
                quiz.Grade = grade;
                _context.Update(quiz);
                await _context.SaveChangesAsync();
            }
            // Otherwise dont give a penalty to the grade
            else if(quiz.AfterEndDateUpload == false && DateTime.Now < quiz.EndDate)
            {
                double grade = ((double)correctAnswers / model.Questions.Count) * 9.00 + 1.00;
                quiz.Grade = grade;
                _context.Update(quiz);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Homework));
        }

        [HttpGet]
        public async Task<IActionResult> ViewGrades()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            // Get the student disciplines for this student if they re an user
            var student = await _context.Students
                                 .Include(sd => sd.StudentDisciplines)
                                 .ThenInclude(d => d.Discipline)
                                 .FirstOrDefaultAsync(s => s.Id == user.Id);
            if(student == null)
                return NotFound();

            var studentGradesViewModel = new StudentGradesViewModel
            {
                StudentName = student.Name,
                Disciplines = student.StudentDisciplines.Select(sd => new DisciplineGradeViewModel
                {
                    DisciplineName = sd.Discipline.Name,
                    GradeAverage = sd.GradeAverage,
                    GeneralGrade = student.GeneralGrade
                }).ToList()
            };

            return View(studentGradesViewModel);

        }
    }
}
