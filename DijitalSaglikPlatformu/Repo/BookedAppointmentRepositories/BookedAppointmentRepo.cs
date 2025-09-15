using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Data;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;
using DijitalSaglikPlatformu.Repo.BookedAppointmentRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DijitalSaglikPlatformu.Repo.BookedAppointmentRepositories
{
    public class BookedAppointmentRepo : IBookedAppointmentRepo
    {


        private readonly UserManager<AppUser> usermanager;
        private readonly AppDbContext context;
        public BookedAppointmentRepo(UserManager<AppUser> UM, AppDbContext AppCon)
        {
            usermanager = UM;
            context = AppCon;
        }







        public async Task<dtoCreateBookAppointment> BookAppointment(dtoCreateBookAppointment dto, string UserId)
        {
            var result = await context.BookedAppointment
                 .Where(x => x.DoctorProfileId == dto.DoctorProfileId &&
                 x.UserId == UserId &&
                 x.AppointmentDate >= DateOnly.FromDateTime(DateTime.UtcNow))
                .FirstOrDefaultAsync();

            if (result != null)
            throw new ArgumentException("You already have an appointment with this doctor.");

            BookedAppointment bookedAppointmentData = new BookedAppointment
            {
                DoctorProfileId=dto.DoctorProfileId,
                UserId= UserId,
                AppointmentDate= dto.AppointmentDate,
                StartTime =dto.StartTime,
                Notes=dto.Notes,
                CreatedAt=DateTime.UtcNow,

            };
          
            await context.BookedAppointment.AddAsync(bookedAppointmentData);
            await context.SaveChangesAsync();
            return dto;

        }




        public async Task<string> CancelAppointment(int Id)
        {
            var AppointmentData = await context.BookedAppointment
                .FirstOrDefaultAsync(x => x.BookedAppointmentId == Id);

            if (AppointmentData == null)
            {
                throw new ArgumentException("This ID is invalid.");
            }

            context.BookedAppointment.Remove(AppointmentData);
            await context.SaveChangesAsync();

            return "Success";


        }





        public async Task<List<dtoGetDoctorAppointments>> GetDoctorAppointments(string UserId)
        {
            var DoctorProfData = await context.DoctorProfile.FirstOrDefaultAsync(x=> x.AppUserId == UserId);
            if (DoctorProfData == null)
            {
                throw new ArgumentException("Please create a Doctor Profile first.");
            }

            var AppointmentsData = await context.BookedAppointment
                .Where(y=>y.DoctorProfileId== DoctorProfData.DoctorProfileId)
                .Join(context.DoctorProfile,
                BA => BA.DoctorProfileId,
                DP => DP.DoctorProfileId,
               (BA, DP) => new { BA, DP })
                .Join(usermanager.Users,
                combined => combined.BA.UserId,
                UM => UM.Id,
                (combined, UM) => new dtoGetDoctorAppointments
                {
                    UserName=UM.UserName,
                    AppointmentDate = combined.BA.AppointmentDate,
                    StartTime= combined.BA.StartTime,
                    Notes= combined.BA.Notes
                } ).ToListAsync();


            return AppointmentsData;

          
        }







        public async Task<List<dtoGetUserAppointments>> GetUserAppointments(string UserId)
        {
            var data=await context.BookedAppointment
                .Where(x => x.UserId == UserId)
                .Join(context.DoctorProfile,
                bookedAppointment => bookedAppointment.DoctorProfileId,
                doctorProfile => doctorProfile.DoctorProfileId,
                (bookedAppointment, doctorProfile) =>new dtoGetUserAppointments
                {
                    FirstName= doctorProfile.FirstName,
                    LastName= doctorProfile.LastName,
                    AppointmentDate= bookedAppointment.AppointmentDate,
                    StartTime= bookedAppointment.StartTime,
                    Location=doctorProfile.Location,
                })
                .ToListAsync();

           
                return data;
        
        }
    }
}
