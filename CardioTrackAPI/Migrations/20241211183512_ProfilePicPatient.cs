using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrackAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePicPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImgUrl",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProfileImgUrlLastGenerationTime",
                table: "Patient",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Patient_DoctorId",
                table: "Patient",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Doctor_DoctorId",
                table: "Patient",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Doctor_DoctorId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_DoctorId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "ProfileImgUrl",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "ProfileImgUrlLastGenerationTime",
                table: "Patient");
        }
    }
}
