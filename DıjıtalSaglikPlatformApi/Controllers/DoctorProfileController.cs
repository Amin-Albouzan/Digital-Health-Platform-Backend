using System.Security.Claims;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Repo.Account;
using DijitalSaglikPlatformu.Repo.DoctorProfileRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DıjıtalSaglikPlatformApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorProfileController : ControllerBase
    {
        private readonly IDoctorProfileRepo doctorProfileRepo;

        public DoctorProfileController(IDoctorProfileRepo repo)
        {
            doctorProfileRepo = repo;
        }

        [HttpGet("GetDoctorProfiles")]
        public async Task<IActionResult> GetDoctorProfiles()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid token or missing user ID.");


                var result = await doctorProfileRepo.GetDoctorProfiles(userId);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                     return Ok(result);
                }
                   
            }

            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error: {errorMessage}");
            }


        }




        [HttpPost("CreateDoctorProfiles")]
        public async Task<IActionResult> CreateDoctorProfiles(dtoDoctorProfile dtoDoctorProfile)
        {
            try
            {
                if(ModelState.IsValid)
                {

                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Unauthorized("Invalid token or missing user ID.");

                    var result = await doctorProfileRepo.
                        CreateDoctorProfiles(dtoDoctorProfile,userId);
                    if(result==null)
                    {
                        return BadRequest();
                    }

                    else
                    {
                        return Ok();
                    }

                   

                }

                else
                {
                    return BadRequest(ModelState);
                }
               
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error: {errorMessage}");
            }

           
        }


        [HttpPut("UpdateDoctorProfiles")]
        public async Task<IActionResult> UpdateDoctorProfiles(dtoUpdateDoctorProfile dtoUpdateDoctorProfile)
        {
           if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


           try
            {
                var result = await doctorProfileRepo.
                     UpdateDoctorProfiles(dtoUpdateDoctorProfile);
                if (result == null)
                {
                    return BadRequest("Doctor profile not found.");
                }

                else
                {
                    return Ok();
                }




            }

            catch(Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error: {errorMessage}");
            }



        }


    }
}
