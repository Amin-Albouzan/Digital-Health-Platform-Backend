using DijitalSaglikPlatformu.Repo.AvailabilityRepositories;
using DijitalSaglikPlatformu.Repo.DoctorWeeklyScheduleRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DıjıtalSaglikPlatformApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {

        private readonly IAvailabilityRepo availabilityRepo;

        public AvailabilityController(IAvailabilityRepo repo)
        {
            availabilityRepo = repo;
        }

        [Authorize(Roles = "Doctor,User")]
        [HttpGet("available-days")]
        public async Task<IActionResult> GetAvailableDays([FromQuery] int doctorId)
        {
            if (doctorId <= 0 )
                return BadRequest("Invalid parameters.");

            var days = await availabilityRepo.GetAvailableDaysForDoctor(doctorId);

            if (!days.Any())
                return NotFound("No available days found for this doctor.");

            return Ok(days.Select(d => d.ToString("yyyy-MM-dd")));
        }


        [Authorize(Roles = "Doctor,User")]
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] int doctorId, [FromQuery] DateTime date)
        {
            if (doctorId <= 0 || date == default)
                return BadRequest("Invalid parameters.");

            var slots = await availabilityRepo.GetAvailableSlotsForDoctor(doctorId, date);

            if (!slots.Any())
                return NotFound("No available slots found for this doctor on the given date.");

            return Ok(slots);
        }

    }
}
