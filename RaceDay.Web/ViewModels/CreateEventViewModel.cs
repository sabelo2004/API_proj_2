using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.ViewModels
{
    public class CreateEventViewModel
    {
        [Required]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        // This will hold the uploaded image file
        public IFormFile? BannerImage { get; set; }
    }
}