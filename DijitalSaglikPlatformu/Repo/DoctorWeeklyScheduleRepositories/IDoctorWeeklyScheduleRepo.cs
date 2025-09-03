using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Dto;

namespace DijitalSaglikPlatformu.Repo.DoctorWeeklyScheduleRepositories
{
    public interface IDoctorWeeklyScheduleRepo
    {

        public Task<List<dtoGetDoctorWeeklyScheduleByDoctorId>> GetDoctorWeeklyScheduleByDoctorId(string UserId);
        public Task<dtoCreateDoctorWeeklySchedule> CreateDoctorWeeklySchedule(dtoCreateDoctorWeeklySchedule dto, string userId);
        public Task<dtoUpdateDoctorWeeklySchedule> UpdateDoctorWeeklySchedule(dtoUpdateDoctorWeeklySchedule dto);
        public Task<string> DeleteDoctorWeeklySchedule(int Id);


    }
}
