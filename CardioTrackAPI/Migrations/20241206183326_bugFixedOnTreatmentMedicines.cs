using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrackAPI.Migrations
{
    /// <inheritdoc />
    public partial class bugFixedOnTreatmentMedicines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentMedicine_Treatment_TreatmentId",
                table: "TreatmentMedicine");

            migrationBuilder.DropColumn(
                name: "TreatmetId",
                table: "TreatmentMedicine");

            migrationBuilder.AlterColumn<long>(
                name: "TreatmentId",
                table: "TreatmentMedicine",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentMedicine_Treatment_TreatmentId",
                table: "TreatmentMedicine",
                column: "TreatmentId",
                principalTable: "Treatment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentMedicine_Treatment_TreatmentId",
                table: "TreatmentMedicine");

            migrationBuilder.AlterColumn<long>(
                name: "TreatmentId",
                table: "TreatmentMedicine",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "TreatmetId",
                table: "TreatmentMedicine",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentMedicine_Treatment_TreatmentId",
                table: "TreatmentMedicine",
                column: "TreatmentId",
                principalTable: "Treatment",
                principalColumn: "Id");
        }
    }
}
