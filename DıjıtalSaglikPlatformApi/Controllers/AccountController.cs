using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;
using DijitalSaglikPlatformu.Repo.Account;
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


           catch (Exception ex)
            {

                return BadRequest(ex);
            }

            
        }


    }
}
