using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Dto
{
    public class dtoGetUserAppointments
    {
        public int BookedAppointmentId { get; set; }
        public int DoctorProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public string Location { get; set; }
        public string Specialty { get; set; }
        public string PhoneNumber { get; set; }

    }
}
