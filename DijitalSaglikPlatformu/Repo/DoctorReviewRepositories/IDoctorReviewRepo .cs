using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Dto;

namespace DijitalSaglikPlatformu.Repo.DoctorReviewRepositories
{
    public interface IDoctorReviewRepo
    {
        public Task<string> CreateReviewAsync(dtoCreateDoctorReview dto, string userId);


    }
}
