using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API_proj_2.Data;
using API_proj_2.Models;

namespace API_proj_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public EnrolmentsController(ApplicationDBContext context) => _context = context;

        public class EnrolDto { public int CategoryID { get; set; } }

        /// <summary>Enrols the logged-in Participant. Participant role only.</summary>
        [HttpPost("/api/events/{eventId}/enrol")]
        [Authorize(Roles = "Participant")]
        public IActionResult Enrol(int eventId, EnrolDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (_context.Events.Find(eventId) == null) return NotFound("Event not found");

            var cat = _context.Categories.Find(dto.CategoryID);
            if (cat == null || cat.EventID != eventId) return NotFound("Category not found");

            if (_context.EventEnrolments.Any(e => e.UserID == userId && e.EventID == eventId))
                return Conflict("Already enrolled in this event");

            var enrolment = new EventEnrolment
            {
                UserID = userId,
                EventID = eventId,
                CategoryID = dto.CategoryID,
                PaymentStatus = "Pending",
                BibNumber = "BIB" + (1000 + _context.EventEnrolments.Count() + 1)
            };

            _context.EventEnrolments.Add(enrolment);
            _context.SaveChanges();
            return StatusCode(201, enrolment);
        }

        /// <summary>Lists the logged-in user's own enrolments.</summary>
        [HttpGet("/api/users/me/enrolments")]
        [Authorize]
        public IActionResult GetMine()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(_context.EventEnrolments.Where(e => e.UserID == userId).ToList());
        }

        /// <summary>Lists all enrolments for an event. Organiser must own it.</summary>
        [HttpGet("/api/events/{eventId}/enrolments")]
        [Authorize(Roles = "Organiser")]
        public IActionResult GetForEvent(int eventId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ev = _context.Events.Find(eventId);
            if (ev == null) return NotFound();
            if (ev.OrganiserID != userId) return Forbid();

            return Ok(_context.EventEnrolments.Where(e => e.EventID == eventId).ToList());
        }

        /// <summary>Cancels own enrolment. Participant only.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Participant")]
        public IActionResult Cancel(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var e = _context.EventEnrolments.Find(id);
            if (e == null) return NotFound();
            if (e.UserID != userId) return Forbid();

            _context.EventEnrolments.Remove(e);
            _context.SaveChanges();
            return Ok("Cancelled");
        }
    }
}