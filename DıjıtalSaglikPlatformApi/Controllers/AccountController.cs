using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;
using DijitalSaglikPlatformu.Repo.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DıjıtalSaglikPlatformApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepo accountRepo;
        private readonly IConfiguration configuration;

        public AccountController(IAccountRepo repo, IConfiguration Con)
        {
            accountRepo = repo;
            this.configuration = Con;

        }


        [HttpGet("CheckToken")]
        [Authorize]
        public IActionResult CheckToken()
        {
            return Ok(new { message = "Token is valid" });
        }



        [HttpPost("RegisterNewUser/{role}")]
        public async Task<IActionResult> RegisterNewUser([FromRoute] string role, [FromBody] RegisterNewUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (role != "User" && role != "Doctor")
                    {
                        return BadRequest("There is an error in the role");
                    }

                   var result= await accountRepo.Register(user, role);

                    if(result== "Success")
                    {
                     return Ok("Success");
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                    
                }
                return BadRequest(ModelState);
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



        [HttpPost("Login")]
        public async Task<IActionResult> Login(dtoLogin login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await accountRepo.Login(login);

                    if (result is string message && message == "Invalid username or password")
                    {
                        return Unauthorized(message);
                    }

                    return Ok(result);

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



        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  var result= await accountRepo.CreateAccessTokenFromRefreshToken(token);

                    if (result is string message && message == "Invalid or expired refresh token")
                    {
                        return Unauthorized();
                    }

                    return Ok(result);
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




        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] dtoResetPassword dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
              var result = await accountRepo.ResetPassword(dto);

                return Ok(result);
            }

            catch(ArgumentException ex) 
            {

                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(errorMessage);

            }


            

        }









        }
}
