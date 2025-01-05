using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        #region Private properties
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        #endregion

        #region Contructors
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                // Create a new user with the form data submitted by the user
                // TODO: Change this to create a Student or Professor object based on the selected role
                // Also need to add additional properties to the RegisterViewModel to capture the additional data
                // from the Student or Professor.
                // I think we should create a separate view model for Student and Professor
                // and on this Register function we can have just a role selection and then a continue button
                // which will send the user to the appropriate view to fill in the additional data
                // and then click on register.
                User user = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };

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
                    foreach (var error in registerAttempt.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
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
