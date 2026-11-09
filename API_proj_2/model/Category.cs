using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_proj_2.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public int EventID { get; set; } // fk event eventID
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        public decimal DistanceKm { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal EntryFee { get; set; }

        public int MaxParticipants { get; set; }
    }
}