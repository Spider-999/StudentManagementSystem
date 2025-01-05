using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedComputerScienceDiscipline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Disciplines",
                columns: new[] { "Id", "GradeAverage", "Name" },
                values: new object[] { "3", null, "ComputerScience" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "3");
        }
    }
}
