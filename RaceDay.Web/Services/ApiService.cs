using System.Text.Json;
using RaceDay.Web.ViewModels;
using System.Net.Http.Json;

namespace RaceDay.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LoginAsync(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result != null)
                {
                    _httpContextAccessor.HttpContext?.Session.SetString("UserRole", result.Role);
                    _httpContextAccessor.HttpContext?.Session.SetString("UserEmail", result.Email);
                    _httpContextAccessor.HttpContext?.Session.SetString("UserName", result.FullName);
                    _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", "CookieAuthLoggedIn");

                    return "Success";
                }
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return $"Login Failed: {response.StatusCode} - {errorContent}";
        }

        public async Task<string> RegisterAsync(RegisterViewModel model)
        {
            var apiDto = new
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber
            };

            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", apiDto);

            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return $"Registration Failed: {response.StatusCode} - {errorContent}";
        }

        public async Task<List<EventViewModel>> GetEventsAsync()
        {
            var response = await _httpClient.GetAsync("api/Events");
            if (response.IsSuccessStatusCode)
            {
                var events = await response.Content.ReadFromJsonAsync<List<EventViewModel>>();
                return events ?? new List<EventViewModel>();
            }
            return new List<EventViewModel>();
        }

        public async Task<EventViewModel?> GetEventByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Events/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<EventViewModel>();
            }
            return null;
        }

        public async Task<List<CategoryViewModel>> GetCategoriesByEventIdAsync(int eventId)
        {
            var response = await _httpClient.GetAsync($"api/events/{eventId}/categories");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>() ?? new List<CategoryViewModel>();
            }
            return new List<CategoryViewModel>();
        }

        public async Task<string> EnrolAsync(int eventId, int categoryId)
        {
            var enrolDto = new
            {
                CategoryID = categoryId
            };

            var response = await _httpClient.PostAsJsonAsync($"api/events/{eventId}/enrol", enrolDto);

            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return $"Enrolment Failed: {response.StatusCode} - {errorContent}";
        }

        public async Task<List<MyEnrolmentsViewModel>> GetMyEnrolmentsAsync()
        {
            var response = await _httpClient.GetAsync("api/users/me/enrolments");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<MyEnrolmentsViewModel>>() ?? new List<MyEnrolmentsViewModel>();
            }
            return new List<MyEnrolmentsViewModel>();
        }

        public async Task<string> CreateEventAsync(CreateEventViewModel model)
        {
            var content = new MultipartFormDataContent();

            // Add text fields
            content.Add(new StringContent("0"), "EventID");
            content.Add(new StringContent("1"), "OrganiserID"); // Hardcoded for now, ideally from session
            content.Add(new StringContent(model.EventName), "EventName");
            content.Add(new StringContent(model.EventDate.ToString("o")), "EventDate");
            content.Add(new StringContent(model.Location), "Location");
            content.Add(new StringContent(model.Description), "Description");
            content.Add(new StringContent("Upcoming"), "Status");

            // Add the image file if one was uploaded
            if (model.BannerImage != null && model.BannerImage.Length > 0)
            {
                var fileContent = new StreamContent(model.BannerImage.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.BannerImage.ContentType);
                content.Add(fileContent, "BannerImage", model.BannerImage.FileName);
            }

            var response = await _httpClient.PostAsync("api/Events", content);

            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return $"Failed to create event: {response.StatusCode} - {errorContent}";
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }
    }

    public class LoginResponse
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}