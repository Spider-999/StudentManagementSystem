using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class StudentDisciplineDummyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Disciplines",
                columns: new[] { "Id", "GradeAverage", "Name" },
                values: new object[] { "2", null, "Physics" });

            migrationBuilder.InsertData(
                table: "StudentDisciplines",
                columns: new[] { "DisciplineId", "StudentId" },
                values: new object[] { "1", "df114734-138a-438a-9e96-12d02427a538" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "StudentDisciplines",
                keyColumns: new[] { "DisciplineId", "StudentId" },
                keyValues: new object[] { "1", "df114734-138a-438a-9e96-12d02427a538" });
        }
    }
}
