using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkForceGovProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCitizenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Citizens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Citizens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "Citizens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Citizens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Citizens",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Citizens");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Citizens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
