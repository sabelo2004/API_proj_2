using API_proj_2.Data;
using API_proj_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_proj_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _environment; // Used to find the wwwroot path

        public EventsController(ApplicationDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            return @event;
        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromForm] EventDto dto, IFormFile? BannerImage)
        {
            var newEvent = new Event
            {
                EventName = dto.EventName,
                EventDate = dto.EventDate,
                Location = dto.Location,
                Description = dto.Description,
                Status = dto.Status,
                OrganiserID = dto.OrganiserID
            };

            // Handle Local Image Upload (to wwwroot)
            if (BannerImage != null && BannerImage.Length > 0)
            {
                try
                {
                    // 1. Get the path to wwwroot/images
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // 2. Create a unique filename
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(BannerImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // 3. Save the file to the hard drive
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await BannerImage.CopyToAsync(stream);
                    }

                    // 4. Save the relative URL to the database
                    // (The Web App will prepend the API base URL to this)
                    newEvent.BannerImageUrl = $"/images/{fileName}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Image Upload Failed: {ex.Message}");
                }
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return StatusCode(201, newEvent);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event @event)
        {
            if (id != @event.EventID) return BadRequest();

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }
    }

    public class EventDto
    {
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int OrganiserID { get; set; }
    }
}