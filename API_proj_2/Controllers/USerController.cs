using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API_proj_2.Data;
using API_proj_2.Models;

namespace API_proj_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UsersController(ApplicationDBContext context) => _context = context;

        /// <summary>Returns the logged-in user's profile.</summary>
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            return Ok(new { user.UserID, user.FullName, user.Email, user.Role, user.PhoneNumber, user.CreatedAt });
        }

        /// <summary>Updates the logged-in user's profile.</summary>
        [HttpPut("me")]
        public IActionResult UpdateMe(UpdateProfileDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            _context.SaveChanges();

            return Ok(new { user.UserID, user.FullName, user.Email, user.Role, user.PhoneNumber });
        }
    }
}