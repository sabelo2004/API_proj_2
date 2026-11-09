using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;

namespace RaceDay.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

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
                var result = await _apiService.LoginAsync(model);

                if (result == "Success")
                {
                    var role = HttpContext.Session.GetString("UserRole");
                    if (role == "Organiser")
                        return RedirectToAction("Dashboard", "Organiser");
                    else
                        return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", result);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.RegisterAsync(model);

                if (result == "Success")
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", result);
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            _apiService.Logout();
            return RedirectToAction("Login");
        }
    }
}