using System.ComponentModel.DataAnnotations;

namespace API_proj_2.Models
{
    public class UpdateProfileDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
    }
}