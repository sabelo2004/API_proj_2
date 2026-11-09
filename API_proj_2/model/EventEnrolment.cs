using System.ComponentModel.DataAnnotations;

namespace API_proj_2.Models
{
    public class EventEnrolment
    {
        [Key]
        public int EnrolmentID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public DateTime EnrolmentDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending";

        [MaxLength(10)]
        public string? BibNumber { get; set; }
    }
}