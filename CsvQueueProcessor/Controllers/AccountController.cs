using CsvQueueProcessor.Core.Entities;
using CsvQueueProcessor.Core.Interfaces;
using CsvQueueProcessor.Infrastructure.Repositories;
using CsvQueueProcessor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CsvQueueProcessor.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { Username = model.Username, Password = model.Password, Email = model.Email };
                var result  = await _userService.AddUserAsync(user);
                if (result > 0)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByCredentialsAsync(model.Username, model.Password);
                if (user != null)
                {
                    // Here you can set up authentication cookies or session
                    // For example:
                    // HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Index", "FileManagement");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear session or authentication cookies here
            /*HttpContext.Session.Remove("Username");*/ // Remove the session variable
                                                    // Alternatively, if you're using cookies:
                                                    // HttpContext.SignOutAsync(); // Uncomment if using cookie authentication

            return RedirectToAction("Login", "Account"); // Redirect to home or login page after logout
        }
    }
}
