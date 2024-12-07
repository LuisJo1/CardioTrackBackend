using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrackAPI.Migrations
{
    /// <inheritdoc />
    public partial class newRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_PersonalBackground_PersonalBackgroundId",
                table: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_Exam_PersonalBackgroundId",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "PersonalBackgroundId",
                table: "Exam");

            migrationBuilder.AddColumn<long>(
                name: "ExamId",
                table: "PersonalBackground",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBackground_ExamId",
                table: "PersonalBackground",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalBackground_Exam_ExamId",
                table: "PersonalBackground",
                column: "ExamId",
                principalTable: "Exam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalBackground_Exam_ExamId",
                table: "PersonalBackground");

            migrationBuilder.DropIndex(
                name: "IX_PersonalBackground_ExamId",
                table: "PersonalBackground");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "PersonalBackground");

            migrationBuilder.AddColumn<long>(
                name: "PersonalBackgroundId",
                table: "Exam",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Exam_PersonalBackgroundId",
                table: "Exam",
                column: "PersonalBackgroundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_PersonalBackground_PersonalBackgroundId",
                table: "Exam",
                column: "PersonalBackgroundId",
                principalTable: "PersonalBackground",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
