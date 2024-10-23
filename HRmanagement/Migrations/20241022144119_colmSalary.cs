using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRmanagement.Migrations
{
    /// <inheritdoc />
    public partial class colmSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSalary",
                table: "salaries");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "salaries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonus",
                table: "salaries",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "salaries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "BaseSalary",
                table: "salaries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Deduction",
                table: "salaries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "salaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaveTaken",
                table: "salaries",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Designations",
                columns: new[] { "Id", "Level", "Name" },
                values: new object[,]
                {
                    { 5, 3, "Senior Account" },
                    { 6, 3, "Project Manager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "BaseSalary",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "Deduction",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "LeaveTaken",
                table: "salaries");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "salaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonus",
                table: "salaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "salaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSalary",
                table: "salaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
