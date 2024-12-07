using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrackAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewTreatmentFKWithPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInDays",
                table: "TreatmentMedicine",
                newName: "Duration");

            migrationBuilder.RenameColumn(
                name: "DurationInWeeks",
                table: "Treatment",
                newName: "Duration");

            migrationBuilder.AddColumn<string>(
                name: "DurationParatemeter",
                table: "TreatmentMedicine",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "TreatmetId",
                table: "TreatmentMedicine",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DurationParameter",
                table: "Treatment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PatientId",
                table: "Treatment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_PatientId",
                table: "Treatment",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatment_Patient_PatientId",
                table: "Treatment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatment_Patient_PatientId",
                table: "Treatment");

            migrationBuilder.DropIndex(
                name: "IX_Treatment_PatientId",
                table: "Treatment");

            migrationBuilder.DropColumn(
                name: "DurationParatemeter",
                table: "TreatmentMedicine");

            migrationBuilder.DropColumn(
                name: "TreatmetId",
                table: "TreatmentMedicine");

            migrationBuilder.DropColumn(
                name: "DurationParameter",
                table: "Treatment");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Treatment");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "TreatmentMedicine",
                newName: "DurationInDays");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Treatment",
                newName: "DurationInWeeks");
        }
    }
}
