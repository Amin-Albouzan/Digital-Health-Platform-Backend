using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Models
{
    public class BookedAppointment
    {

        [Key]
        public int BookedAppointmentId { get; set; }


        [ForeignKey("DoctorProfile")]
        public int DoctorProfileId { get; set; }


        
        public string UserId { get; set; }

        [Column(TypeName = "date")]
        public DateOnly AppointmentDate { get; set; }


        public DateTime StartTime { get; set; }


        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }


      
        public DoctorProfile Doctor { get; set; }

        
        

    }
}
