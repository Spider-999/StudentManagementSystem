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

        #region Index and ViewStudents Methods
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
            // Get all of the students from the database and include the student disciplines and disciplines and put them in a list
            var students = await _context.Students.
                                   Include(sd => sd.StudentDisciplines).
                                   ThenInclude(d => d.Discipline).
                                   ToListAsync();

            if (students == null)
                return RedirectToAction("Index");

            // Calculate the grade average for each student based on the discipline grade formula
            foreach (var student in students)
            {
                foreach (var studentDiscipline in student.StudentDisciplines)
                {
                    studentDiscipline.GradeAverage = CalculateGradeAverage(student, studentDiscipline.Discipline);
                    _logger.LogInformation($"Updated grade average for {student.Name} in {studentDiscipline.Discipline.Name}: {studentDiscipline.GradeAverage}");
                }
                student.GeneralGrade = CalculateGeneralGrade(student);
                _logger.LogInformation($"Updated general grade for {student.Name}: {student.GeneralGrade}");
            }

            await _context.SaveChangesAsync();

            // Pass the student list to the view
            return View(students);
        }
        #endregion

        #region Homework Methods
        [HttpGet]
        public async Task<IActionResult> ViewStudentHomeworks(string studentId)
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if(user == null) 
                return NotFound();
            // Get the student the professor clicked on
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return NotFound();

            var professor = await _context.Professors
                                   .Include(p => p.Discipline)
                                   .FirstOrDefaultAsync(p => p.Id == user.Id);
            if(professor == null)
                return NotFound();

            // Get the student's homeworks but only for the professors discipline
            // A math professor shouldnt be able to view and grade physics homework.
            var homeworks = await _context.Homeworks
                .Where(s => s.StudentId == studentId 
                && s.DisciplineId == professor.DisciplineId)
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
                        CreationDate = DateTime.Now,
                        EndDate = model.EndDate,
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
                            CreationDate = DateTime.Now,
                            EndDate = model.EndDate,
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
                            CreationDate = DateTime.Now,
                            EndDate = model.EndDate,
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
                Content = homework.Content,
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
                updatedHomeworkTemplate.Content = model.Content;
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
                    homework.Content = model.Content;
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
            if (projectFiles == null)
                return NotFound();
            // To read and write the byte array data of the file conten
            using (var memoryStream = new MemoryStream())
            {
                // Use zip archive to download all of the project files for the professor
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create))
                {
                    foreach (var file in projectFiles)
                    {
                        // Create the zip archive with the name of the file
                        var zip = zipArchive.CreateEntry(file.FileName, CompressionLevel.Optimal);
                        using (var zipStream = zip.Open())
                        {
                            await zipStream.WriteAsync(file.FileContent, 0, file.FileContent.Length);
                        }
                    }
                }
                // Create a random name for the zip file
                var zipName = Path.GetTempFileName() + ".zip";
                // Return the file with the specified MIME(indicates the type of a document)
                // type of application/zip
                return File(memoryStream.ToArray(), "applcation/zip", zipName);
            }
        }
    

        [HttpGet]
        public IActionResult CreateQuiz()
        {
            return View(new CreateQuizViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(CreateQuizViewModel model)
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
            
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return NotFound();
                
                // Find the professor and include the discipline in the query because we need it for 
                // the quiz creation
                var professor = await _context.Professors
                                      .Include(d => d.Discipline)
                                      .FirstOrDefaultAsync(p => p.Id == user.Id);
                if (professor == null)
                    return NotFound();


                var students = await _context.Students.ToListAsync();
                if (students == null)
                    return NotFound();

                // Add template quiz homework for the professor view
                Quiz templateQuiz = new Quiz
                {
                    Title = model.Title,
                    Description = model.Description,
                    CreationDate = DateTime.Now,
                    EndDate = model.EndDate,
                    Grade = 0.00,
                    Status = false,
                    Mandatory = model.Mandatory,
                    Penalty = model.Penalty,
                    AfterEndDateUpload = model.AfterEndUploadDate,
                    DisciplineId = professor.DisciplineId,
                    TimeLimit = model.TimeLimit,
                    IsTemplate = true
                };
                _context.Quizzes.Add(templateQuiz);
                await _context.SaveChangesAsync();

                // Add quiz questions for the template quiz
                foreach(var questionModel in model.Questions)
                {
                    var quizQuestion = new QuizQuestion
                    {
                        Question = questionModel.Question,
                        Answers = questionModel.Answers,
                        CorrectAnswer = questionModel.CorrectAnswer,
                        QuizID = templateQuiz.Id
                    };
                    _context.QuizQuestions.Add(quizQuestion);
                }
                await _context.SaveChangesAsync();

                // Add quizzes and questions to the quizes of the students
                foreach(var student in students)
                {
                    // Create a quiz for the student
                    Quiz quiz = new Quiz
                    {
                        Title = model.Title,
                        Description = model.Description,
                        CreationDate = DateTime.Now,
                        EndDate = model.EndDate,
                        Grade = 0.00,
                        Status = false,
                        Mandatory = model.Mandatory,
                        AfterEndDateUpload = model.AfterEndUploadDate,
                        Penalty = model.Penalty,
                        StudentId = student.Id,
                        DisciplineId = professor.DisciplineId,
                        TimeLimit = model.TimeLimit,
                        IsTemplate = false
                    };
                    _context.Quizzes.Add(quiz);
                    await _context.SaveChangesAsync();

                    // Add quiz questions
                    foreach(var questionModel in model.Questions)
                    {
                        QuizQuestion question = new QuizQuestion
                        {
                            Question = questionModel.Question,
                            Answers = questionModel.Answers,
                            CorrectAnswer = questionModel.CorrectAnswer,
                            QuizID = quiz.Id
                        };

                        _context.QuizQuestions.Add(question);
                    }
                    await _context.SaveChangesAsync();
                }

            return RedirectToAction(nameof(Homeworks));
        }
        #endregion

        #region Grade Methods
        [HttpGet]
        public async Task<IActionResult> GradeHomework(string homeworkId)
        {
            var homework = await _context.Homeworks.FindAsync(homeworkId);
            if (homework == null)
                return NotFound();

            var model = new GradeHomeworkViewModel
            {
                Id = homework.Id,
                Title = homework.Title,
                Description = homework.Description,
                Content = homework.Content,
                Grade = (double)homework.Grade,
                Comment = homework.Comment
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GradeHomework(GradeHomeworkViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var homework = await _context.Homeworks
                                 .Include(h => h.Student)
                                 .FirstOrDefaultAsync(h => h.Id == model.Id);
            if (homework == null)
                return NotFound();

            homework.Grade = model.Grade;
            homework.Comment = model.Comment;
            homework.Status = true;
            _context.Update(homework);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewStudentHomeworks), new { studentId = homework.StudentId });
        }

        [HttpGet]
        public async Task<IActionResult> GradeCalculationFormula(string disciplineId)
        {
            var discipline = await _context.Disciplines.FindAsync(disciplineId);
            if (discipline == null)
                return NotFound();

            var model = new GradeCalculationFormulaViewModel
            {
                DisciplineID = discipline.Id,
                Formula = discipline.GradeCalculationFormula
            };

            ViewBag.DisciplineName = discipline.Name;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GradeCalculationFormula(GradeCalculationFormulaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var discipline = await _context.Disciplines.FindAsync(model.DisciplineID);
            if (discipline == null)
                return NotFound();

            discipline.GradeCalculationFormula = model.Formula;
            _context.Update(discipline);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public double CalculateGradeAverage(Student student, Discipline discipline)
        {
            // Get all of the homeworks that have a grade higher than 0(graded homeworks)
            var homeworks = _context.Homeworks.Where(h => h.StudentId == student.Id
                                                     && h.DisciplineId == discipline.Id
                                                     && h.Grade.HasValue).
                                                     ToList();

            if (homeworks == null)
                return 0.00;

            double gradeAverage = 0.00;
            switch (discipline.GradeCalculationFormula)
            {
                case "MA1":
                    gradeAverage = homeworks.Sum(h => h.Grade.Value) / homeworks.Count;
                    break;

                case "MA2":
                    gradeAverage = homeworks.Sum(h => h.Grade.Value) / homeworks.Count;
                    gradeAverage = Math.Ceiling(gradeAverage);
                    break;

                case "MA3":
                    gradeAverage = homeworks.Sum(h => h.Grade.Value) / homeworks.Count;
                    gradeAverage = Math.Floor(gradeAverage);
                    break;

                default:
                    gradeAverage = homeworks.Sum(h => h.Grade.Value) / homeworks.Count;
                    break;
            }

            // Repalce NaN values with 0.00 to be able to save to database
            if (double.IsNaN(gradeAverage))
                gradeAverage = 0.00;

            return Math.Round(gradeAverage,2);
        }

        public double CalculateGeneralGrade(Student student)
        {
            var studentDisciplines = student.StudentDisciplines;

            if (studentDisciplines.Count == 0)
                return 0.00;

            double generalGrade = 0.00;
            foreach (var studentDiscipline in studentDisciplines)
            {
                if (studentDiscipline.GradeAverage.HasValue && studentDiscipline.GradeAverage.Value > 0)
                {
                    generalGrade += studentDiscipline.GradeAverage.Value;
                }
            }

            // Repalce NaN values with 0.00 to be able to save to database
            if (double.IsNaN(generalGrade))
                generalGrade = 0.00;

            // The final grade is just the average of all of the discipline average grades
            // divided by the number of student disciplines. And I also rounded it to 2 point precision.
            return Math.Round(generalGrade / studentDisciplines.Count, 2);
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> ExportGradesToCSV()
        {
            var students = await _context.Students
                            .Include(sd => sd.StudentDisciplines)
                            .ThenInclude(d => d.Discipline)
                            .ToListAsync();

            if (students == null)
                return NotFound();

            // Create columns in the data table
            var dataTable = new DataTable();
            dataTable.Columns.Add("StudentName", typeof(string));
            dataTable.Columns.Add("Discipline", typeof(string));
            dataTable.Columns.Add("GradeAverage", typeof(string));
            dataTable.Columns.Add("GeneralGrade", typeof(string));

            foreach (var student in students)
            {
                foreach (var studentDiscipline in student.StudentDisciplines)
                {
                    var gradeAverage = studentDiscipline.GradeAverage;
                    var generalGrade = student.GeneralGrade;

                    dataTable.Rows.Add(student.Name, studentDiscipline.Discipline.Name, gradeAverage, generalGrade);
                }
            }

            // Convert datatable to CSV
            // String builder is used to modify strings without creating new objects + increased performance
            var csvConvert = new StringBuilder();
            foreach(DataColumn column in dataTable.Columns)
                csvConvert.Append(column.ColumnName + ";");
            // Append default line terminator
            csvConvert.AppendLine();

            foreach(DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                    csvConvert.Append(item + ";");
                csvConvert.AppendLine();
            }

            var csvContent = csvConvert.ToString();
            var csvToBytes = Encoding.UTF8.GetBytes(csvContent);
            // Make a random file name
            var csvFileName = $"Grades_{DateTime.Now}.csv";
            return File(csvToBytes, "text/csv", csvFileName);
        }
    }
}
