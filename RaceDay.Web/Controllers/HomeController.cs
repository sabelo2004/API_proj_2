using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch events from the API
            var events = await _apiService.GetEventsAsync();
            return View(events);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}