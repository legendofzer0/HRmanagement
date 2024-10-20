using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRmanagement.Migrations
{
    /// <inheritdoc />
    public partial class addSalaryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Timings");

            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "Timings");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "Timings");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Timings");

            migrationBuilder.DropColumn(
                name: "TotalSalary",
                table: "Timings");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "salaries");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Timings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Timings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Timings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "salaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Bonus",
                table: "salaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "salaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "salaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSalary",
                table: "salaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Timings");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Timings");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Timings");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "TotalSalary",
                table: "salaries");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Timings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Bonus",
                table: "Timings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "Timings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Timings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSalary",
                table: "Timings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
