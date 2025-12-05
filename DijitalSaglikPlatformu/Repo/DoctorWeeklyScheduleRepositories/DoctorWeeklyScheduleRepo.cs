using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Data;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DijitalSaglikPlatformu.Repo.DoctorWeeklyScheduleRepositories
{
    public class DoctorWeeklyScheduleRepo : IDoctorWeeklyScheduleRepo
    {



        private readonly UserManager<AppUser> usermanager;
        private readonly AppDbContext context;
        public DoctorWeeklyScheduleRepo(UserManager<AppUser> UM, AppDbContext AppCon)
        {
            usermanager = UM;
            context = AppCon;
        }


        public async Task<dtoCreateDoctorWeeklySchedule> CreateDoctorWeeklySchedule(dtoCreateDoctorWeeklySchedule dto, string userId)
        {
            var doctorProf = await context.DoctorProfile.FirstOrDefaultAsync(d => d.AppUserId == userId);
            if (doctorProf == null)
                throw new ArgumentException("You must create a doctor profile first.");


            if (dto.StartTime >= dto.EndTime)
                throw new ArgumentException("StartTime must be earlier than EndTime.");

            var existing = await context.DoctorWeeklySchedule
                .AnyAsync(s => s.DoctorProfileId == doctorProf.DoctorProfileId && s.DayOfWeek == dto.DayOfWeek);

            if (existing)
                throw new Exception("This day is already scheduled.");

            DoctorWeeklySchedule doctorWeeklySchedule = new DoctorWeeklySchedule
            {
                DoctorProfileId = doctorProf.DoctorProfileId,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                SlotDurationInMinutes = dto.SlotDurationInMinutes,
            };

            await context.DoctorWeeklySchedule.AddAsync(doctorWeeklySchedule);
            await context.SaveChangesAsync();

            return dto;
        }

        public async Task<string> DeleteDoctorWeeklySchedule(int id)
        {
            var schedule = await context.DoctorWeeklySchedule.FindAsync(id);

            if (schedule == null)
                throw new KeyNotFoundException("Schedule not found.");

            context.DoctorWeeklySchedule.Remove(schedule);
            await context.SaveChangesAsync();

            return "Success";
        }


        public async Task<List<dtoGetDoctorWeeklyScheduleByDoctorId>> GetDoctorWeeklyScheduleByDoctorId(string userId)
        {
            var doctor = await context.DoctorProfile
                .FirstOrDefaultAsync(x => x.AppUserId == userId);

            if (doctor == null)
                throw new ArgumentException("Doctor not found for the given user ID.");

            var schedules = await context.DoctorWeeklySchedule
                .Where(x => x.DoctorProfileId == doctor.DoctorProfileId)
                .Select(x => new dtoGetDoctorWeeklyScheduleByDoctorId
                {
                    Id = x.Id,
                    DayOfWeek = x.DayOfWeek,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    SlotDurationInMinutes = x.SlotDurationInMinutes
                })
                .ToListAsync();

            if (!schedules.Any())
                throw new InvalidOperationException("No schedule found for the doctor.");

            return schedules;
        }



        public async Task<dtoUpdateDoctorWeeklySchedule> UpdateDoctorWeeklySchedule(dtoUpdateDoctorWeeklySchedule dto)
        {
            if (dto.StartTime >= dto.EndTime)
                throw new ArgumentException("StartTime must be earlier than EndTime.");

            var OldDoctorWeeklyScheduleData =await context.DoctorWeeklySchedule.FindAsync(dto.Id);

            if (OldDoctorWeeklyScheduleData == null)
                return null;

            var exists = await context.DoctorWeeklySchedule
          .AnyAsync(d =>
         d.DoctorProfileId == OldDoctorWeeklyScheduleData.DoctorProfileId &&
         d.DayOfWeek == dto.DayOfWeek &&
         d.Id != dto.Id);     

            if (exists)
                throw new Exception("This day is already scheduled for the doctor.");


            OldDoctorWeeklyScheduleData.DayOfWeek= dto.DayOfWeek;
            OldDoctorWeeklyScheduleData.StartTime = dto.StartTime;
            OldDoctorWeeklyScheduleData.EndTime = dto.EndTime;
            OldDoctorWeeklyScheduleData.SlotDurationInMinutes = dto.SlotDurationInMinutes;

            await context.SaveChangesAsync();

            return dto;
        }
    }
}
