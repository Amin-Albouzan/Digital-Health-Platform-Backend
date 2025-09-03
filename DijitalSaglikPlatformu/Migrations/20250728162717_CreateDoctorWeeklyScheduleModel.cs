using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DijitalSaglikPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class CreateDoctorWeeklyScheduleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorWeeklySchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorProfileId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SlotDurationInMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorWeeklySchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorWeeklySchedule_DoctorProfile_DoctorProfileId",
                        column: x => x.DoctorProfileId,
                        principalTable: "DoctorProfile",
                        principalColumn: "DoctorProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorWeeklySchedule_DoctorProfileId",
                table: "DoctorWeeklySchedule",
                column: "DoctorProfileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorWeeklySchedule");
        }
    }
}
