using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DijitalSaglikPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDoctorWeeklyScheduleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorWeeklySchedule_DoctorProfileId",
                table: "DoctorWeeklySchedule");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorWeeklySchedule_DoctorProfileId",
                table: "DoctorWeeklySchedule",
                column: "DoctorProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorWeeklySchedule_DoctorProfileId",
                table: "DoctorWeeklySchedule");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorWeeklySchedule_DoctorProfileId",
                table: "DoctorWeeklySchedule",
                column: "DoctorProfileId",
                unique: true);
        }
    }
}
