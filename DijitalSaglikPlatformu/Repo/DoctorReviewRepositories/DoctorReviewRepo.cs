using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Data;
using DijitalSaglikPlatformu.Dto;
using DijitalSaglikPlatformu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DijitalSaglikPlatformu.Repo.DoctorReviewRepositories
{
    public class DoctorReviewRepo : IDoctorReviewRepo
    {

        private readonly UserManager<AppUser> usermanager;
        private readonly AppDbContext context;
        public DoctorReviewRepo(UserManager<AppUser> UM, AppDbContext AppCon)
        {
            usermanager = UM;
            context = AppCon;
        }





        public async Task<string> CreateReviewAsync(dtoCreateDoctorReview dto, string userId)
        {

            // 1️⃣ تحقق من وجود الطبيب
            var doctor = await context.DoctorProfile
                .FirstOrDefaultAsync(d => d.DoctorProfileId == dto.DoctorProfileId);
            if (doctor == null)
                throw new ArgumentException("Invalid doctor ID.");

            // 2️⃣ تحقق أن المريض زار الطبيب فعلاً
            var hasVisited = await context.BookedAppointment
                .AnyAsync(b =>
                    b.DoctorProfileId == dto.DoctorProfileId &&
                    b.UserId == userId &&
                    b.StartTime < DateTime.UtcNow);

             if (!hasVisited)
             throw new ArgumentException("You cannot review this doctor because you haven't visited yet.");

            // 3️⃣ تحقق أنه لم يقم بتقييم نفس الطبيب من قبل
            var alreadyReviewed = await context.DoctorReview
                .AnyAsync(r =>
                    r.DoctorProfileId == dto.DoctorProfileId &&
                    r.UserId == userId);

            if (alreadyReviewed)
                throw new ArgumentException("You have already reviewed this doctor.");

            // 4️⃣ إنشاء المراجعة
            var review = new DoctorReview
            {
                DoctorProfileId = dto.DoctorProfileId,
                UserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await context.DoctorReview.AddAsync(review);
            await context.SaveChangesAsync();

            // 5️⃣ تحديث متوسط التقييم للطبيب بعد إضافة التقييم الجديد
            var ratings = await context.DoctorReview
                .Where(r => r.DoctorProfileId == dto.DoctorProfileId)
                .Select(r => r.Rating)
                .ToListAsync();

            var newAverage = ratings.Any() ? ratings.Average() : 0;
            doctor.AverageRating = newAverage;

            context.DoctorProfile.Update(doctor);
            await context.SaveChangesAsync();

            return "Review added successfully.";





        }
    }
}
