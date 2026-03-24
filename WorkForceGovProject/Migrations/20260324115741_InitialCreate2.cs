using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkForceGovProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmploymentPrograms",
                columns: table => new
                {
                    ProgramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentPrograms", x => x.ProgramID);
                });

            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    BenefitID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProgramID = table.Column<int>(type: "int", nullable: false),
                    CitizenID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.BenefitID);
                    table.ForeignKey(
                        name: "FK_Benefits_Citizens_CitizenID",
                        column: x => x.CitizenID,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Benefits_EmploymentPrograms_ProgramID",
                        column: x => x.ProgramID,
                        principalTable: "EmploymentPrograms",
                        principalColumn: "ProgramID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    ResourceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProgramID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceID);
                    table.ForeignKey(
                        name: "FK_Resources_EmploymentPrograms_ProgramID",
                        column: x => x.ProgramID,
                        principalTable: "EmploymentPrograms",
                        principalColumn: "ProgramID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    TrainingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProgramID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.TrainingID);
                    table.ForeignKey(
                        name: "FK_Trainings_EmploymentPrograms_ProgramID",
                        column: x => x.ProgramID,
                        principalTable: "EmploymentPrograms",
                        principalColumn: "ProgramID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_CitizenID",
                table: "Benefits",
                column: "CitizenID");

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_ProgramID",
                table: "Benefits",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ProgramID",
                table: "Resources",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_ProgramID",
                table: "Trainings",
                column: "ProgramID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Trainings");

            migrationBuilder.DropTable(
                name: "EmploymentPrograms");
        }
    }
}
