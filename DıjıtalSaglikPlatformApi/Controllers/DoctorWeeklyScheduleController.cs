using DijitalSaglikPlatformu.Dto;
using System.Security.Claims;
using DijitalSaglikPlatformu.Repo.DoctorProfileRepositories;
using DijitalSaglikPlatformu.Repo.DoctorWeeklyScheduleRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DıjıtalSaglikPlatformApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorWeeklyScheduleController : ControllerBase
    {

        private readonly IDoctorWeeklyScheduleRepo doctorWeeklyScheduleRepo;

        public DoctorWeeklyScheduleController(IDoctorWeeklyScheduleRepo repo)
        {
            doctorWeeklyScheduleRepo = repo;
        }


        [Authorize(Roles = "Doctor")]
        [HttpPost("CreateDoctorWeeklySchedule")]
        public async Task<IActionResult> CreateDoctorWeeklySchedule([FromBody] dtoCreateDoctorWeeklySchedule dtoCreateDoctorWeekly)
        {
            try
            {
            
                if (ModelState.IsValid)
                {

                    var userId =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Unauthorized("Invalid token or missing user ID.");

                    var result = await doctorWeeklyScheduleRepo.
                        CreateDoctorWeeklySchedule(dtoCreateDoctorWeekly, userId);

                 

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
                return BadRequest(ex.Message);
            }


        }



        [Authorize(Roles = "Doctor")]
        [HttpPut("UpdateDoctorWeeklySchedule")]
        public async Task<IActionResult> UpdateDoctorWeeklySchedule([FromBody] dtoUpdateDoctorWeeklySchedule dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result=await doctorWeeklyScheduleRepo.UpdateDoctorWeeklySchedule(dto);

                  

                if (result == null)
                    return NotFound("Schedule not found.");

                return Ok(result); 

            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



          

         
        }



        [Authorize(Roles = "Doctor")]
        [HttpGet("GetDoctorWeeklyScheduleByDoctorId")]
        public async Task<IActionResult> GetDoctorWeeklyScheduleByDoctorId()
        {
            try
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid token or missing user ID.");

                var result = await doctorWeeklyScheduleRepo.GetDoctorWeeklyScheduleByDoctorId(userId);

              

                if (result == null)
                    return NotFound("Schedule not found.");

                return Ok(result);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }




        [Authorize(Roles = "Doctor")]
        [HttpDelete("DeleteDoctorWeeklySchedule/{id}")]
        public async Task<IActionResult> DeleteDoctorWeeklySchedule(int id)
        {
            try
            {
                var result = await doctorWeeklyScheduleRepo.DeleteDoctorWeeklySchedule(id);
                return Ok(result); 
            }
            catch (KeyNotFoundException ex)
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
