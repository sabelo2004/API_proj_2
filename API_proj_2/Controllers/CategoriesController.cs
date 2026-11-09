using API_proj_2.Data;
using API_proj_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_proj_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public CategoriesController(ApplicationDBContext context) => _context = context;

        /// <summary>Lists categories for an event. Public endpoint.</summary>
        [HttpGet("/api/events/{eventId}/categories")]
        [AllowAnonymous]
        public IActionResult GetForEvent(int eventId)
        {
            if (_context.Events.Find(eventId) == null) return NotFound();
            return Ok(_context.Categories.Where(c => c.EventID == eventId).ToList());
        }

        /// <summary>Adds a category. Organiser must own the event.</summary>
        [HttpPost("/api/events/{eventId}/categories")]
        [Authorize(Roles = "Organiser")]
        public IActionResult Create(int eventId, Category newCategory)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ev = _context.Events.Find(eventId);
            if (ev == null) return NotFound();
            if (ev.OrganiserID != userId) return Forbid();

            newCategory.EventID = eventId;
            newCategory.CategoryID = 0;
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return StatusCode(201, newCategory);
        }

        /// <summary>Updates a category. Organiser must own parent event.</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public IActionResult Update(int id, Category updated)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cat = _context.Categories.Find(id);
            if (cat == null) return NotFound();
            var ev = _context.Events.Find(cat.EventID);
            if (ev == null || ev.OrganiserID != userId) return Forbid();

            cat.CategoryName = updated.CategoryName;
            cat.DistanceKm = updated.DistanceKm;
            cat.EntryFee = updated.EntryFee;
            cat.MaxParticipants = updated.MaxParticipants;
            _context.SaveChanges();
            return Ok(cat);
        }

        /// <summary>Deletes a category. Organiser must own parent event.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cat = _context.Categories.Find(id);
            if (cat == null) return NotFound();
            var ev = _context.Events.Find(cat.EventID);
            if (ev == null || ev.OrganiserID != userId) return Forbid();

            _context.Categories.Remove(cat);
            _context.SaveChanges();
            return Ok("Deleted");
        }
    }
}