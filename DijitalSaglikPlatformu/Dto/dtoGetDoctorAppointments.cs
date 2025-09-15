using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Dto
{
    public class dtoGetDoctorAppointments
    {

        public string UserName { get; set; }

        [Column(TypeName = "date")]
        public DateOnly AppointmentDate { get; set; }

        public DateTime StartTime { get; set; }


        [MaxLength(500)]
        public string? Notes { get; set; }

    }
}
