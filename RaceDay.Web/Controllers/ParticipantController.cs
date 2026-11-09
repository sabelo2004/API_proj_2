using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;

namespace RaceDay.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // UPDATED: Now fetches enrolments from the API
        public async Task<IActionResult> MyEnrolments()
        {
            var enrolments = await _apiService.GetMyEnrolmentsAsync();
            return View(enrolments);
        }

        [HttpGet]
        public async Task<IActionResult> EventDetails(int id)
        {
            var eventDetails = await _apiService.GetEventByIdAsync(id);
            if (eventDetails == null) return NotFound();

            var categories = await _apiService.GetCategoriesByEventIdAsync(id);

            var viewModel = new EventDetailsViewModel
            {
                Event = eventDetails,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            var result = await _apiService.EnrolAsync(eventId, categoryId);

            if (result == "Success")
            {
                TempData["SuccessMessage"] = "Successfully enrolled in the event!";
                return RedirectToAction("MyEnrolments");
            }

            TempData["ErrorMessage"] = result;
            return RedirectToAction("EventDetails", new { id = eventId });
        }
    }
}