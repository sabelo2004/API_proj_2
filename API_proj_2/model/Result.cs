using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_proj_2.Models
{
    public class Result
    {
        public int ResultID { get; set; }

        public int EnrolmentID { get; set; } // set from the route by the API

        // Nullable: a DNF or DQ has no finish time or position (matches your SQL script)
        [Column(TypeName = "time")]
        public TimeSpan? FinishTime { get; set; }

        public int? Position { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression("^(Finished|DNF|DQ)$", ErrorMessage = "Status must be Finished, DNF or DQ.")]
        public string Status { get; set; } = "Finished";
    }
}