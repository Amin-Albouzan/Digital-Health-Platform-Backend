using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Models
{
    public class DoctorProfile
    {
        [Key]
        public int DoctorProfileId { get; set; }

        [Required]
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        
        [Required,MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Range(20, 100, ErrorMessage = "Age must be between 20 and 100.")]
        public int Age { get; set; }


        [Required]
        public string Specialty { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public byte[] PhotoUrl { get; set; }
          
        [Required]
        public string PhotoContentType { get; set; } 


        [Required]
        public string City { get; set; }

        [Required]
        public string Location { get; set; }

        [Required,Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }


        public double AverageRating { get; set; } = 0;


        public DateTime CreatedAt { get; set; } 

        public DateTime? UpdatedAt { get; set; }





        public AppUser AppUser { get; set; }
        public List<DoctorWeeklySchedule> DoctorWeeklySchedule { get; set; }
        public List<BookedAppointment> BookedAppointment { get; set; }

    }
}
