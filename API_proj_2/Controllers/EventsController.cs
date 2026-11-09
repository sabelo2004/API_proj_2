using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API_proj_2.Data;
using API_proj_2.Models;

namespace API_proj_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public EventsController(ApplicationDBContext context) => _context = context;

        /// <summary>Lists all events. Public endpoint.</summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetEvents() => Ok(_context.Events.ToList());

        /// <summary>Returns a single event by ID. Public endpoint.</summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetEvent(int id)
        {
            var ev = _context.Events.Find(id);
            return ev == null ? NotFound() : Ok(ev);
        }

        /// <summary>Creates a new event. Organiser role required.</summary>
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public IActionResult CreateEvent(Event newEvent)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            newEvent.OrganiserID = userId;
            newEvent.EventID = 0;

            _context.Events.Add(newEvent);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventID }, newEvent);
        }

        /// <summary>Updates an event. Organiser must own it.</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public IActionResult UpdateEvent(int id, Event updated)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ev = _context.Events.Find(id);
            if (ev == null) return NotFound();
            if (ev.OrganiserID != userId) return Forbid();

            ev.EventName = updated.EventName;
            ev.EventDate = updated.EventDate;
            ev.Location = updated.Location;
            ev.Description = updated.Description;
            ev.Status = updated.Status;
            _context.SaveChanges();
            return Ok(ev);
        }

        /// <summary>Deletes an event. Organiser must own it.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public IActionResult DeleteEvent(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ev = _context.Events.Find(id);
            if (ev == null) return NotFound();
            if (ev.OrganiserID != userId) return Forbid();

            _context.Events.Remove(ev);
            _context.SaveChanges();
            return Ok("Deleted");
        }
    }
}