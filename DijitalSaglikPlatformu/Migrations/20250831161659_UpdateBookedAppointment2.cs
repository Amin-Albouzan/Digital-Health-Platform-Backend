using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DijitalSaglikPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookedAppointment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedAppointment_AspNetUsers_AppUserId",
                table: "BookedAppointment");

            migrationBuilder.DropIndex(
                name: "IX_BookedAppointment_AppUserId",
                table: "BookedAppointment");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BookedAppointment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "BookedAppointment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookedAppointment_AppUserId",
                table: "BookedAppointment",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedAppointment_AspNetUsers_AppUserId",
                table: "BookedAppointment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
