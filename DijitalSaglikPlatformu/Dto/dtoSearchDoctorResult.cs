using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Dto
{
    public class dtoSearchDoctorResult
    {
        public int DoctorProfileId { get; set; }
        public string FirstName { get; set; }


        public string LastName { get; set; }

        public int Age { get; set; }


        public string Specialty { get; set; }

        public string Description { get; set; }

        public string PhotoBase64 { get; set; }

        public string PhotoContentType { get; set; }

        public double AverageRating { get; set; }
        public string City { get; set; }

        public string Location { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }



    }
}
