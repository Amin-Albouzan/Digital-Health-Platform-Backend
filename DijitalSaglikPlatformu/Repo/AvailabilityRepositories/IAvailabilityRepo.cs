using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalSaglikPlatformu.Repo.AvailabilityRepositories
{
    public interface IAvailabilityRepo
    {
        public Task<List<DateTime>> GetAvailableDaysForDoctor(int doctorId);

        public Task<List<object>> GetAvailableSlotsForDoctor(int doctorId, DateTime date);
    }
}
