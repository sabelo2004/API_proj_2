using System.ComponentModel.DataAnnotations;

namespace API_proj_2.Models
{
    public class Route
    {
        public int RouteID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        [MaxLength(100)]
        public string RouteName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? RouteMapURL { get; set; }

        public int? ElevationGainM { get; set; }
    }
}