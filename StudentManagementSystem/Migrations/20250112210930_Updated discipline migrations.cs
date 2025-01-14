using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Updateddisciplinemigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Disciplines",
                columns: new[] { "Id", "GradeAverage", "Name" },
                values: new object[,]
                {
                    { "1", null, "Mathematics" },
                    { "2", null, "Physics" },
                    { "3", null, "ComputerScience" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "3");
        }
    }
}
