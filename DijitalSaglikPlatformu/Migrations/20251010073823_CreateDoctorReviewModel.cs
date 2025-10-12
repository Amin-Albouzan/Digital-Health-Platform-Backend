using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DijitalSaglikPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class CreateDoctorReviewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorReview",
                columns: table => new
                {
                    DoctorReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorProfileId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorReview", x => x.DoctorReviewId);
                    table.ForeignKey(
                        name: "FK_DoctorReview_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorReview_DoctorProfile_DoctorProfileId",
                        column: x => x.DoctorProfileId,
                        principalTable: "DoctorProfile",
                        principalColumn: "DoctorProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorReview_DoctorProfileId",
                table: "DoctorReview",
                column: "DoctorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorReview_UserId",
                table: "DoctorReview",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorReview");
        }
    }
}
