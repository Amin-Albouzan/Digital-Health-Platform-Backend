using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;

namespace DijitalSaglikPlatformu.Repo.DoctorProfileRepositories
{
    public interface IDoctorProfileRepo
    {
        public Task<dtoGetDoctorProfile> GetDoctorProfiles(string UserId);
        public Task<List<dtoGetDoctorProfile>> GetAllDoctorProfiles();
        public Task<dtoDoctorProfile> CreateDoctorProfiles(dtoDoctorProfile dto,string userId);
        public Task<dtoUpdateDoctorProfile> UpdateDoctorProfiles(dtoUpdateDoctorProfile dto);

    }
}
