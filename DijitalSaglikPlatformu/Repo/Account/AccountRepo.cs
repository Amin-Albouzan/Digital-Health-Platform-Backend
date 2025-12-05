using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Data;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DijitalSaglikPlatformu.Repo.Account
{
    public class AccountRepo : IAccountRepo
    {

        private readonly UserManager<AppUser> usermanager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly AppDbContext context;
        public AccountRepo(UserManager<AppUser> UM, RoleManager<IdentityRole> RM, IConfiguration Con, AppDbContext AppCon)
        {
            usermanager = UM;
            roleManager = RM;
            configuration = Con;
            context = AppCon;
        }


        public async Task<string> Login(dtoLogin login)
        {

            AppUser? user = await usermanager.FindByNameAsync(login.UserName);

            if (user != null)
            {
                if (await usermanager.CheckPasswordAsync(user, login.Password))
                {
                    var claims = new List<Claim>();
                    // claims.Add(new Claim("name", "value"));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var roles = await usermanager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    // signingCredentials
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        claims: claims,
                        issuer: configuration["JWT:Issuer"],
                        audience: configuration["JWT:Audience"],
                        expires: DateTime.UtcNow.AddMinutes(15),
                        signingCredentials: sc
                    );


                    // 🟡 إنشاء Refresh Token
                    var refreshTokenStr = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                    var refreshToken = new RefreshToken
                    {
                        Token = refreshTokenStr,
                        ExpiresOn = DateTime.UtcNow.AddDays(7),
                        CreatedOn=DateTime.UtcNow,
                        UserId=user.Id

                    };

                    // 🟢 حفظ الـ RefreshToken في قاعدة البيانات
                    context.RefreshTokens.Add(refreshToken);
                    await context.SaveChangesAsync();




                    var _token = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        RefreshToken= refreshTokenStr,
                        Role= roles

                    };
                    return JsonSerializer.Serialize(_token);
                }
                else
                {
                    return "Invalid username or password";
                }
            }
            else
            {
                return "Invalid username or password";
            }


        }

      

        public async Task<string> Register(RegisterNewUser userData,string role)
        {

            AppUser? TestEmail = await usermanager.FindByEmailAsync(userData.Email);
            if(TestEmail!=null)
            {
               throw new ArgumentException("This email address is already in use.");
            }


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


        public async Task<string> CreateAccessTokenFromRefreshToken(string refreshTokenStr)
        {
                var token = await context.RefreshTokens
                 .Include(t => t.User)
                 .FirstOrDefaultAsync(t => t.Token == refreshTokenStr && t.RevokedOn == null);

                if (token == null || token.ExpiresOn < DateTime.UtcNow)
                    return ("Invalid or expired refresh token");


                token.RevokedOn = DateTime.UtcNow;

                var newRefreshToken = new RefreshToken
                {
                    Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                    ExpiresOn = DateTime.UtcNow.AddDays(7),
                    CreatedOn = DateTime.UtcNow,
                    UserId = token.UserId
                };
                context.RefreshTokens.Add(newRefreshToken);

                var claims = new List<Claim>
            {
             new Claim(ClaimTypes.NameIdentifier, token.User.Id),
             new Claim(ClaimTypes.Name, token.User.UserName),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


                var roles = await usermanager.GetRolesAsync(token.User);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var newAccessToken = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
                );

                string accessTokenStr = new JwtSecurityTokenHandler().WriteToken(newAccessToken);

                await context.SaveChangesAsync(); // حفظ التعديلات


                var _token = new
                {
                    token = accessTokenStr,
                    expiration = newAccessToken.ValidTo,
                    RefreshToken = newRefreshToken.Token,

                };
                return JsonSerializer.Serialize(_token);

            

           

        }



       

        public async Task<string> ResetPassword(dtoResetPassword dto)
        {
            var UserData = await usermanager.FindByEmailAsync(dto.Email);
            if (UserData == null)
                throw new ArgumentException("Invalid Email");

           if(UserData.UserName!=dto.UserName)
                throw new ArgumentException("Invalid Name ");


            var removePasswordResult = await usermanager.RemovePasswordAsync(UserData);
            if (!removePasswordResult.Succeeded)
                 throw new ArgumentException("Failed to remove old password.");


            var addPasswordResult = await usermanager.AddPasswordAsync(UserData, dto.NewPassword);
            if (!addPasswordResult.Succeeded)
                throw new ArgumentException("Failed to set new password.");


            return("Password reset successfully.");
        }




    }
}
