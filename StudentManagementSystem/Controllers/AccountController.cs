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
            // Check if the model binding and form submission is valid
            if (ModelState.IsValid)
            {
                User user;
                // TODO: Move this logic elsewhere, maybe make a 
                // static UserAccountCreation class with methods for 
                // both Student and Professor.
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
                        // Get all of the disciplines in the database
                        var disciplines = await _context.Disciplines.ToListAsync();
                        string id = string.Empty;
                        // Search for the selected department of the user(discipline)
                        // and get the id of that discipline.
                        foreach(Discipline disc in disciplines)
                        {
                            if (disc.Name == model.Department)
                            {
                                id = disc.Id;
                                break;
                            }
                        }
                        user = new Professor
                        {
                            Name = model.Name,
                            Email = model.Email,
                            UserName = model.Email,
                            Department = model.Department,
                            DisciplineId = id
                        };
                        break;
                    default:
                        throw new Exception("Registration went wrong!");
                        break;
                }

                // Attempt to create the user in the database and assign the selecte role to the user
                var registerAttempt = await _userManager.CreateAsync(user, model.Password);
                var roleAssignAttempt = await _userManager.AddToRoleAsync(user, model.Role);

                if (registerAttempt.Succeeded && roleAssignAttempt.Succeeded)
                {
                    // Redirect the user to the login page if successful
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // Otherwise, display the errors to the user
                    foreach(var error in roleAssignAttempt.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            // If the model binding and form submission is not valid, return the view with the model
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
                    // TODO: Redirect user to their specific view: StudentView, ProfessorView or AdminView
                    return RedirectToAction("Index", "Home");
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
    }
}
