using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Models
{
    public class DoctorWeeklySchedule
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("DoctorProfile")]
        public int DoctorProfileId { get; set; }

       
        [Required]
        public DayOfWeek DayOfWeek { get; set; } // Monday, Tuesday, etc.

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public int SlotDurationInMinutes { get; set; } 



        public DoctorProfile DoctorProfile { get; set; }

    }
}
