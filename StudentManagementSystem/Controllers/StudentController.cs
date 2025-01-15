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
            if (ModelState.IsValid)
            {
                var updatedHomework = await _context.Homeworks.FindAsync(homework.Id);
                if (updatedHomework == null)
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

            try
            {
                if (!ModelState.IsValid)
                {
                    //Exception thrown if the data sent is not valid
                    throw new StudentControllerException("Invalid data", 400);
                }
                var project = await _context.Projects.FindAsync(model.ProjectID);
                if (project == null)
                    return NotFound();

                // Iterate through all of the models files sent by the user
                foreach (var file in model.Files)
                {
                    // To read and write the byte array data of the file content
                    using (var memoryStream = new MemoryStream())
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

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Homework));

            }
            catch (StudentControllerException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }
    }
}
