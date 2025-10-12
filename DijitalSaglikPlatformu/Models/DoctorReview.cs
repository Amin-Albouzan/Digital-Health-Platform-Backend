using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Models
{
    public class DoctorReview
    {


        [Key]
        public int DoctorReviewId { get; set; }

        [Required]
        [ForeignKey("DoctorProfile")]
        public int DoctorProfileId { get; set; }

        [Required]
        [ForeignKey("AppUser")]
        public string UserId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



        public DoctorProfile DoctorProfile { get; set; }
        public AppUser AppUser { get; set; }




    }
}
