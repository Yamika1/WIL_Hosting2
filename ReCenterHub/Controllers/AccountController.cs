using Microsoft.AspNetCore.Mvc;
using ReCenterHub.Services;
using ReCenterHub.ViewModels;

namespace ReCenterHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiAuthService _apiAuthService;

        public AccountController(ApiAuthService apiAuthService)
        {

            _apiAuthService = apiAuthService;
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);

            }

            var result = await _apiAuthService.LoginAsync(model.Email, model.Password);

            if (result == null || string.IsNullOrEmpty(result.AccessToken))
            {

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");

                return View(model);

            }

            HttpContext.Session.SetString("AccessToken", result.AccessToken);

            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public IActionResult Register()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);

            }
            var result = await _apiAuthService.RegisterAsync(model.Email, model.Password);

            if (result)

            {

                return RedirectToAction("Login", "Account");

            }

            ModelState.AddModelError(string.Empty, "Registration failed.");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("AccessToken");

            return RedirectToAction("Index", "Home");
        }
    }
}