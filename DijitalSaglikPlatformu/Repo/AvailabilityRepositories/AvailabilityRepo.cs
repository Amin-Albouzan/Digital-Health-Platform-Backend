using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Data;
using DijitalSaglikPlatformu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DijitalSaglikPlatformu.Repo.AvailabilityRepositories
{
    public class AvailabilityRepo : IAvailabilityRepo
    {


        private readonly UserManager<AppUser> usermanager;
        private readonly AppDbContext context;
        public AvailabilityRepo(UserManager<AppUser> UM, AppDbContext AppCon)
        {
            usermanager = UM;
            context = AppCon;
        }








        public async Task<List<DateTime>> GetAvailableDaysForDoctor(int DoctorProfileId)
        {
            var schedules = await context.DoctorWeeklySchedule
                   .Where(s => s.DoctorProfileId == DoctorProfileId)
                   .ToListAsync();

            if (!schedules.Any())
                return new List<DateTime>();

            var startDate = DateTime.Today;           // يبدأ من اليوم الحالي
            var endDate = startDate.AddMonths(3);     // ينتهي بعد 3 شهور

            var availableDays = new List<DateTime>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var daySchedule = schedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);
                if (daySchedule == null) continue;

                //  توليد كل الـ Slots
                var totalSlots = new List<DateTime>();
                var currentTime = date.Date + daySchedule.StartTime;
                var endTime = date.Date + daySchedule.EndTime;

                while (currentTime < endTime)
                {
                    totalSlots.Add(currentTime);
                    currentTime = currentTime.AddMinutes(daySchedule.SlotDurationInMinutes);
                }

                // الحجوزات لذلك اليوم
                var booked = await context.BookedAppointment
                    .Where(b => b.DoctorProfileId == DoctorProfileId &&
                                b.StartTime.Date == date.Date)
                    .Select(b => b.StartTime)
                    .ToListAsync();

                var freeSlots = totalSlots.Except(booked).ToList();

                if (freeSlots.Any())
                    availableDays.Add(date);
            }

            return availableDays;


        }

        public async Task<List<object>> GetAvailableSlotsForDoctor(int doctorId, DateTime date)
        {
            var schedule = await context.DoctorWeeklySchedule
        .FirstOrDefaultAsync(s => s.DoctorProfileId == doctorId && s.DayOfWeek == date.DayOfWeek);

            if (schedule == null)
                return new List<object>();

            var totalSlots = new List<(DateTime start, DateTime end)>();
            var currentTime = date.Date + schedule.StartTime;
            var endTime = date.Date + schedule.EndTime;

            while (currentTime < endTime)
            {
                totalSlots.Add((currentTime, currentTime.AddMinutes(schedule.SlotDurationInMinutes)));
                currentTime = currentTime.AddMinutes(schedule.SlotDurationInMinutes);
            }

            var booked = await context.BookedAppointment
                .Where(b => b.DoctorProfileId == doctorId && b.StartTime.Date == date.Date)
                .Select(b => b.StartTime)
                .ToListAsync();

            var freeSlots = totalSlots
              .Where(slot => !booked.Any(b =>
               b.Hour == slot.start.Hour && b.Minute == slot.start.Minute
               ))
               .Select(slot => new
               {
               startTime = slot.start.ToString("HH:mm"),
               endTime = slot.end.ToString("HH:mm")
                })
               .ToList<object>();


            return freeSlots;
        }
    }
}
