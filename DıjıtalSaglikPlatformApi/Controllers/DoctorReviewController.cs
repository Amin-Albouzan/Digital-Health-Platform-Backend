using DijitalSaglikPlatformu.Dto;
using System.Security.Claims;
using DijitalSaglikPlatformu.Repo.BookedAppointmentRepositories;
using DijitalSaglikPlatformu.Repo.DoctorReviewRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DıjıtalSaglikPlatformApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorReviewController : ControllerBase
    {

        private readonly IDoctorReviewRepo doctorReviewRepo;

        public DoctorReviewController(IDoctorReviewRepo repo)
        {
            doctorReviewRepo = repo;
        }


        [Authorize(Roles = "User")]
        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateReview([FromBody] dtoCreateDoctorReview dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid token.");

                var result = await doctorReviewRepo.CreateReviewAsync(dto, userId);

                if (result != "Review added successfully.")
                    return BadRequest(result);

                return Ok(result);
            }

            catch (ArgumentException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(errorMessage);
            }




            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error: {errorMessage}");
            }
        }

    }
}
