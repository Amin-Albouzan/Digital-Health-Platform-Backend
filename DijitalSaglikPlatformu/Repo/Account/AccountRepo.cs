using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Data;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;
using Microsoft.AspNetCore.Identity;

namespace DijitalSaglikPlatformu.Repo.Account
{
    public class AccountRepo : IAccountRepo
    {

        private readonly UserManager<AppUser> usermanager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepo(UserManager<AppUser> UM, RoleManager<IdentityRole> RM)
        {
            usermanager = UM;
            roleManager = RM;
        }


        public string Login()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegisterNewUser userData,string role)
        {
            AppUser appUser = new()
            {
                UserName = userData.UserName,
                Email = userData.Email,
            };

            IdentityResult result = await usermanager.CreateAsync(appUser, userData.Password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    return "Role does not exist";
                await usermanager.AddToRoleAsync(appUser, role);
                return ("Success");
            }

            else
            {
                return string.Join(" | ", result.Errors.Select(e => e.Description));
            }

        }
    }
}
