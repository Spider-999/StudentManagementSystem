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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Check if the model binding and form submission is valid
            if (ModelState.IsValid)
            {
                // Create a new user with the form data submitted by the user
                User user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                // Attempt to create the user in the database
                var registerAttempt = await _userManager.CreateAsync(user, model.Password);

                if (registerAttempt.Succeeded)
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
