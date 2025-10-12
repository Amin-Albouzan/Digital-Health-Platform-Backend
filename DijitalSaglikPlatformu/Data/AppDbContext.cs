using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DijitalSaglikPlatformu.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<DoctorProfile> DoctorProfile { get; set; }
        public DbSet<DoctorWeeklySchedule> DoctorWeeklySchedule { get; set; }
        public DbSet<BookedAppointment> BookedAppointment { get; set; }
        public DbSet<DoctorReview> DoctorReview { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookedAppointment>()
               .HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
          .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<DoctorReview>()
        .HasOne(r => r.DoctorProfile)
        .WithMany()
        .HasForeignKey(r => r.DoctorProfileId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorReview>()
                .HasOne(r => r.AppUser)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);


        }


    }
}
