using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRmanagement.Migrations
{
    /// <inheritdoc />
    public partial class SeedDesignationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Designations",
                columns: new[] { "Id", "Level", "Name" },
                values: new object[,]
                {
                    { 1, 3, "Senier Dot Net Developer" },
                    { 2, 2, "Mid level Frontend Developer" },
                    { 3, 1, "Junior UI/UX Designer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
