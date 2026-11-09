using API_proj_2.Data;
using API_proj_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace API_proj_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public ResultsController(ApplicationDBContext context) => _context = context;

        /// <summary>Captures a result. Organiser must own the event.</summary>
        [HttpPost("/api/enrolments/{enrolmentId}/result")]
        [Authorize(Roles = "Organiser")]
        public IActionResult Create(int enrolmentId, Result newResult)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var e = _context.EventEnrolments.Find(enrolmentId);
            if (e == null) return NotFound("Enrolment not found");

            var ev = _context.Events.Find(e.EventID);
            if (ev == null || ev.OrganiserID != userId) return Forbid();

            newResult.EnrolmentID = enrolmentId;
            newResult.ResultID = 0;
            _context.Results.Add(newResult);
            _context.SaveChanges();
            return StatusCode(201, newResult);
        }

        /// <summary>Returns results belonging to the logged-in Participant.</summary>
        [HttpGet("/api/results/mine")]
        [Authorize(Roles = "Participant")]
        public IActionResult GetMine()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var enrolmentIds = _context.EventEnrolments
                .Where(e => e.UserID == userId).Select(e => e.EnrolmentID).ToList();
            return Ok(_context.Results.Where(r => enrolmentIds.Contains(r.EnrolmentID)).ToList());
        }

        /// <summary>Returns all results for one event. Organiser must own it.</summary>
        [HttpGet("/api/events/{eventId}/results")]
        [Authorize(Roles = "Organiser")]
        public IActionResult GetForEvent(int eventId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ev = _context.Events.Find(eventId);
            if (ev == null) return NotFound();
            if (ev.OrganiserID != userId) return Forbid();

            var enrolmentIds = _context.EventEnrolments
                .Where(e => e.EventID == eventId).Select(e => e.EnrolmentID).ToList();
            return Ok(_context.Results.Where(r => enrolmentIds.Contains(r.EnrolmentID)).ToList());
        }
    }
}