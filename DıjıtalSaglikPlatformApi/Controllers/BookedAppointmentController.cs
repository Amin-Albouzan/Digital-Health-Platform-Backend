using System.Security.Claims;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Repo.BookedAppointmentRepositories;
using DijitalSaglikPlatformu.Repo.DoctorProfileRepositories;
using DijitalSaglikPlatformu.Repo.DoctorWeeklyScheduleRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DıjıtalSaglikPlatformApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookedAppointmentController : ControllerBase
    {
        private readonly IBookedAppointmentRepo bookedAppointmentRepo;

        public BookedAppointmentController(IBookedAppointmentRepo repo)
        {
            bookedAppointmentRepo = repo;
        }


        [Authorize(Roles = "Doctor,User")]
        [HttpPost("BookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody] dtoCreateBookAppointment dto )
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var userId =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Unauthorized("Invalid token or missing user ID.");

                    var result = await bookedAppointmentRepo.
                        BookAppointment(dto, userId);

                   

                    if (result == null)
                        return NotFound("Schedule not found.");

                    return Ok(result);



                }

                else
                {
                    return BadRequest(ModelState);
                }

            }


            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }





            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error: {errorMessage}");
            }





        }



        [Authorize(Roles = "User")]
        [HttpGet("GetUserAppointments")]
        public async Task<IActionResult> GetUserAppointments()
        {
            var userId =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token or missing user ID.");

            var data=await bookedAppointmentRepo.GetUserAppointments(userId);

            return Ok(data);
        }




        [Authorize(Roles = "Doctor")]
        [HttpGet("GetDoctorAppointments")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid token or missing user ID.");

                var data = await bookedAppointmentRepo.GetDoctorAppointments(userId);

                return Ok(data);
            }



            catch (ArgumentException ex)
            { return BadRequest(ex.Message); }

        }



        [Authorize(Roles ="Doctor,User")]
        [HttpDelete("CancelAppointment/{id}")]
        public async Task<IActionResult> CancelAppointment([FromRoute] int id)
        {
            try
            {
                var result = await bookedAppointmentRepo.CancelAppointment(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error: {errorMessage}");
            }
        }




    }
}
