using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Data;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DijitalSaglikPlatformu.Repo.DoctorProfileRepositories
{
    public class DoctorProfileRepo : IDoctorProfileRepo
    {



        private readonly UserManager<AppUser> usermanager;
        private readonly AppDbContext context;
        public DoctorProfileRepo(UserManager<AppUser> UM, AppDbContext AppCon)
        {
            usermanager = UM;
            context = AppCon;
        }




        //public async Task<List<string>> GetDoctorProfiles(int id)
        //{
        //    var data=await context.DoctorProfile.FirstOrDefaultAsync(x=>x.DoctorProfileId==id);
        //   if (data!=null)
        //    {
        //        dtoDoctorProfile dtoDoctorProfile = new()
        //        {
        //        FirstName=data.FirstName,
        //        LastName=data.LastName,
        //        Age=data.Age,
        //        Specialty=data.Specialty,
        //            Description = data.Description,





        //        };

        //    }
           

        //}

        public async Task<dtoDoctorProfile> CreateDoctorProfiles(dtoDoctorProfile dto,string userId)
        {
            byte[] imageData = null;
            if (dto.PhotoUrl != null && dto.PhotoUrl.Length > 0)
            {
                using var stream = new MemoryStream();
                await dto.PhotoUrl.CopyToAsync(stream);
                imageData = stream.ToArray();

            }
            

            DoctorProfile doctorProfile = new DoctorProfile
            {
                FirstName = dto.FirstName,
                LastName= dto.LastName,
                Age= dto.Age,
                Specialty = dto.Specialty,
                Description = dto.Description,
                PhotoUrl= imageData,
                PhotoContentType = dto.PhotoUrl.ContentType,
                City = dto.City,
                Location= dto.Location,
                PhoneNumber = dto.PhoneNumber,
                Gender = dto.Gender,
                CreatedAt=DateTime.UtcNow,
                AppUserId=userId



            };
            await context.DoctorProfile.AddAsync(doctorProfile);
            await context.SaveChangesAsync();


            return dto;


        }

       

        public async Task<dtoUpdateDoctorProfile> UpdateDoctorProfiles(dtoUpdateDoctorProfile NewDto)
        {
            DoctorProfile OldDoctorProfile = await context.DoctorProfile.FirstOrDefaultAsync(x => x.DoctorProfileId== NewDto.DoctorProfileId);
            if(OldDoctorProfile==null)
            {
                return null;
            }

          
             OldDoctorProfile.FirstName = NewDto.FirstName;
            OldDoctorProfile.LastName = NewDto.LastName;
            OldDoctorProfile.Age = NewDto.Age;
            OldDoctorProfile.Specialty = NewDto.Specialty;
            OldDoctorProfile.Description = NewDto.Description;
            OldDoctorProfile.City = NewDto.City;
            OldDoctorProfile.Location = NewDto.Location;
            OldDoctorProfile.PhoneNumber = NewDto.PhoneNumber;
            OldDoctorProfile.Gender = NewDto.Gender;
            OldDoctorProfile.UpdatedAt = DateTime.UtcNow;

            if (NewDto.PhotoUrl != null && NewDto.PhotoUrl.Length > 0)
            {
           
                using var stream = new MemoryStream();
                await NewDto.PhotoUrl.CopyToAsync(stream);
                OldDoctorProfile.PhotoUrl = stream.ToArray();
            }

            await context.SaveChangesAsync();
            return NewDto;
        }




        

        public async Task<dtoGetDoctorProfile> GetDoctorProfiles(string UserId)
        {
            var data = await context.DoctorProfile
                .Where(x => x.AppUserId == UserId)
                .Select(x => new dtoGetDoctorProfile
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    Specialty = x.Specialty,
                    Description = x.Description,
                    City = x.City,
                    Location = x.Location,
                    PhoneNumber = x.PhoneNumber,
                    Gender = x.Gender,
                    PhotoBase64 = x.PhotoUrl != null
                    ? $"data:{x.PhotoContentType};base64,{Convert.ToBase64String(x.PhotoUrl)}"
                    : null,
                    PhotoContentType = x.PhotoContentType



                })
                .FirstOrDefaultAsync();

            return data;
        }
    }
}
