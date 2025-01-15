using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        #region Private properties
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDatabaseContext _context;
        #endregion

        #region Contructors
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDatabaseContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        #endregion
       
        #region Register methods
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create the user object, see in the helper methods region
                User user = await CreateUserBasedOnRoleAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid registration!");
                    return View(model);
                }

                // Attempt to create the user in the database and assign the selected role to the user
                var registerAttempt = await _userManager.CreateAsync(user, model.Password);
                var roleAssignAttempt = await _userManager.AddToRoleAsync(user, model.Role);
                if (await _userManager.IsInRoleAsync(user, "Student"))
                    await AddStudentDisciplinesOnRegister(user);

                if (registerAttempt.Succeeded && roleAssignAttempt.Succeeded)
                {
                    // Redirect the user to the login page if successful
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // Otherwise, display the errors to the user
                    foreach (var error in roleAssignAttempt.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    // If the model binding and form submission is not valid, return the view with the model
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion

        #region Login methods
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginAttempt = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (loginAttempt.Succeeded)
                {
                    // See in the helper methods region
                    return await RedirectBasedOnRoleAsync(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Datele sunt incorecte");
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion

        #region Logout method
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Helper methods

        private async Task<User> CreateUserBasedOnRoleAsync(RegisterViewModel model)
        {
            User user = null;
            // Create user based on the selected role
            switch (model.Role)
            {
                case "Student":
                    user = new Student
                    {
                        Name = model.Name,
                        Email = model.Email,
                        UserName = model.Email,
                        YearOfStudy = model.YearOfStudy
                    };
                    break;
                case "Professor":
                    // Find all disciplines in the database
                    var discipline = await _context.Disciplines.FirstOrDefaultAsync(d => d.Name == model.Department);
                    if(discipline == null)
                    {
                        ModelState.AddModelError(string.Empty, "Disciplina nu a fost gasita pentru profesor!");
                        return null;
                    }
                    // Create the professor using the found id of the selected
                    // discipline
                    user = new Professor
                    {
                        Name = model.Name,
                        Email = model.Email,
                        UserName = model.Email,
                        Department = model.Department,
                        DisciplineId = discipline.Id
                    };
                    break;
                default:
                    return null;
            }
            return user;
        }

        public async Task AddStudentDisciplinesOnRegister(User student)
        {
                var disciplines = await _context.Disciplines.ToListAsync();
                // At registration add the student to all the student disciplines that exist
                foreach (var discipline in disciplines)
                {
                    var studentDiscipline = new StudentDiscipline
                    {
                        StudentId = student.Id,
                        DisciplineId = discipline.Id
                    };
                    _context.StudentDisciplines.Add(studentDiscipline);
                }
                await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> RedirectBasedOnRoleAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            // If no user was found show the error to the user
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login!");
                return View("Login", model);
            }

            // Find which role the user belongs to and redirect them
            // to their dashboards
            if(await _userManager.IsInRoleAsync(user, "Student"))
            {
                return RedirectToAction("Index", "Student");
            }
            else if(await _userManager.IsInRoleAsync(user, "Professor"))
            {
                return RedirectToAction("Index", "Professor");
            }
            else if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
