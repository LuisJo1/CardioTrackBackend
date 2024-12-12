using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrackAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePicS3KeyPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImgS3Key",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImgS3Key",
                table: "Patient");
        }
    }
}
