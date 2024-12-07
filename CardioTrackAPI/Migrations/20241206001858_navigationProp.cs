using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrackAPI.Migrations
{
    /// <inheritdoc />
    public partial class navigationProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonalBackground_ExamId",
                table: "PersonalBackground");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBackground_ExamId",
                table: "PersonalBackground",
                column: "ExamId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonalBackground_ExamId",
                table: "PersonalBackground");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBackground_ExamId",
                table: "PersonalBackground",
                column: "ExamId");
        }
    }
}
