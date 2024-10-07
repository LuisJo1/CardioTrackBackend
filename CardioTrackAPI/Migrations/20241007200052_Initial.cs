using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardioTrackAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalBackgroundObstetric",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GestationWeeks = table.Column<int>(type: "int", nullable: false),
                    For = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Caesarean = table.Column<bool>(type: "bit", nullable: false),
                    Stillbirth = table.Column<bool>(type: "bit", nullable: false),
                    Abortion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalBackgroundObstetric", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalBackground",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArterialHypertension = table.Column<bool>(type: "bit", nullable: false),
                    MyocardialInfarction = table.Column<bool>(type: "bit", nullable: false),
                    Asthma = table.Column<bool>(type: "bit", nullable: false),
                    Pneumopathy = table.Column<bool>(type: "bit", nullable: false),
                    Dyslipidemia = table.Column<bool>(type: "bit", nullable: false),
                    Angina = table.Column<bool>(type: "bit", nullable: false),
                    LiverDisease = table.Column<bool>(type: "bit", nullable: false),
                    Homeopathy = table.Column<bool>(type: "bit", nullable: false),
                    Stroke = table.Column<bool>(type: "bit", nullable: false),
                    DiabetesMellitus = table.Column<bool>(type: "bit", nullable: false),
                    Chagas = table.Column<bool>(type: "bit", nullable: false),
                    Thyroidopathy = table.Column<bool>(type: "bit", nullable: false),
                    Surgery = table.Column<bool>(type: "bit", nullable: false),
                    SurgeryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObstetricId = table.Column<long>(type: "bigint", nullable: false),
                    MedicineAllergy = table.Column<bool>(type: "bit", nullable: false),
                    Toxics = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalBackground_PersonalBackgroundObstetric_ObstetricId",
                        column: x => x.ObstetricId,
                        principalTable: "PersonalBackgroundObstetric",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    LastPasswordRecoveryTimeUTC = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherPersonalBackground",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalBackgroundId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherPersonalBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherPersonalBackground_PersonalBackground_PersonalBackgroundId",
                        column: x => x.PersonalBackgroundId,
                        principalTable: "PersonalBackground",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalBackgroundMedicine",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalBackgroundId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalBackgroundMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalBackgroundMedicine_PersonalBackground_PersonalBackgroundId",
                        column: x => x.PersonalBackgroundId,
                        principalTable: "PersonalBackground",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalBackgroundMedicineAllergy",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalBackgroundId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalBackgroundMedicineAllergy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalBackgroundMedicineAllergy_PersonalBackground_PersonalBackgroundId",
                        column: x => x.PersonalBackgroundId,
                        principalTable: "PersonalBackground",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalBackgroundToxic",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalBackgroundId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalBackgroundToxic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalBackgroundToxic_PersonalBackground_PersonalBackgroundId",
                        column: x => x.PersonalBackgroundId,
                        principalTable: "PersonalBackground",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Names = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surnames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BornDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctor_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordRecoverToken",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Revoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecoverToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordRecoverToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Names = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surnames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BornDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exam",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    PatientGenre = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    PatientAge = table.Column<int>(type: "int", nullable: false),
                    PersonalBackgroundId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exam_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Exam_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Exam_PersonalBackground_PersonalBackgroundId",
                        column: x => x.PersonalBackgroundId,
                        principalTable: "PersonalBackground",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_UserId",
                table: "Doctor",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_DoctorId",
                table: "Exam",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_PatientId",
                table: "Exam",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_PersonalBackgroundId",
                table: "Exam",
                column: "PersonalBackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherPersonalBackground_PersonalBackgroundId",
                table: "OtherPersonalBackground",
                column: "PersonalBackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordRecoverToken_UserId",
                table: "PasswordRecoverToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_UserId",
                table: "Patient",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBackground_ObstetricId",
                table: "PersonalBackground",
                column: "ObstetricId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBackgroundMedicine_PersonalBackgroundId",
                table: "PersonalBackgroundMedicine",
                column: "PersonalBackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBackgroundMedicineAllergy_PersonalBackgroundId",
                table: "PersonalBackgroundMedicineAllergy",
                column: "PersonalBackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBackgroundToxic_PersonalBackgroundId",
                table: "PersonalBackgroundToxic",
                column: "PersonalBackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RolId",
                table: "User",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exam");

            migrationBuilder.DropTable(
                name: "OtherPersonalBackground");

            migrationBuilder.DropTable(
                name: "PasswordRecoverToken");

            migrationBuilder.DropTable(
                name: "PersonalBackgroundMedicine");

            migrationBuilder.DropTable(
                name: "PersonalBackgroundMedicineAllergy");

            migrationBuilder.DropTable(
                name: "PersonalBackgroundToxic");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "PersonalBackground");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PersonalBackgroundObstetric");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
