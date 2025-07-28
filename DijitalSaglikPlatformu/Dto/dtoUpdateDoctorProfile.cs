using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DijitalSaglikPlatformu.Dto
{
    public class dtoUpdateDoctorProfile
    {
        [Required]
        public int DoctorProfileId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, Range(20, 100, ErrorMessage = "Age must be between 20 and 100.")]
        public int Age { get; set; }


        [Required]
        public string Specialty { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile? PhotoUrl { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Location { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }


      

    }
}
