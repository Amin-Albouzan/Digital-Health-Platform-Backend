using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Dto
{
    public class dtoCreateDoctorReview
    {
        public int DoctorProfileId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}
