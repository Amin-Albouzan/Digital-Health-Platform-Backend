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



        [Authorize(Roles ="Doctor")]
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




        [Authorize(Roles = "Doctor,User")]
        [HttpGet("GetAllDoctorProfiles")]
        public async Task<IActionResult> GetAllDoctorProfiles()
        {
            try
            {
              
                var result = await doctorProfileRepo.GetAllDoctorProfiles();
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




        [Authorize(Roles = "Doctor")]
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

            catch (ArgumentException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest( $"Error: {errorMessage}");
            }



            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error: {errorMessage}");
            }

           
        }



        [Authorize(Roles = "Doctor")]
        [HttpPut("UpdateDoctorProfiles")]
        public async Task<IActionResult> UpdateDoctorProfiles(dtoUpdateDoctorProfile dtoUpdateDoctorProfile)
        {
           if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


           try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid token or missing user ID.");

                var result = await doctorProfileRepo.
                     UpdateDoctorProfiles(dtoUpdateDoctorProfile, userId);
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






        [Authorize(Roles ="Doctor,User")]
        [HttpGet("GetDoctorProfileById/{DoctorProfileId}")]
        public async Task<IActionResult> GetDoctorProfileById([FromRoute] int DoctorProfileId)
        {
            try
            {
                
                var result = await doctorProfileRepo.GetDoctorProfileById(DoctorProfileId);
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






        [Authorize(Roles = "Doctor,User")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchDoctors(
           [FromQuery] string? specialty,
           [FromQuery] string? city,
           [FromQuery] string? gender,
           [FromQuery] double? minRating)
        {
            try
            {
                var doctors = await doctorProfileRepo.SearchDoctorsAsync(specialty, city, gender, minRating);

                if (doctors == null || !doctors.Any())
                    return NotFound("No doctors found with these filters.");

                return Ok(doctors);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error: {errorMessage}");
            }
        }



    }
}
