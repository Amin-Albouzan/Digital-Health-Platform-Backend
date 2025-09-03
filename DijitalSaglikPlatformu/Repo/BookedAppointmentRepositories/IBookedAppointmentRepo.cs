using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Dto;

namespace DijitalSaglikPlatformu.Repo.BookedAppointmentRepositories
{
    public interface IBookedAppointmentRepo
    {
        public Task<dtoCreateBookAppointment> BookAppointment(dtoCreateBookAppointment dto,string UserId);

        public Task<List<dtoGetUserAppointments>>GetUserAppointments(string UserId);

        public Task<List<dtoGetDoctorAppointments>> GetDoctorAppointments(string UserId);

        public Task<string> CancelAppointment(int Id);

    }
}
