using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Dto
{
    public class dtoUpdateDoctorWeeklySchedule
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; } // Monday, Tuesday, etc.

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int SlotDurationInMinutes { get; set; }


    }
}
