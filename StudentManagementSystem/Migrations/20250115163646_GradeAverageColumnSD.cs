using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class GradeAverageColumnSD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GradeAverage",
                table: "StudentDisciplines",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "1",
                column: "GradeCalculationFormula",
                value: "MA1");

            migrationBuilder.UpdateData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "2",
                column: "GradeCalculationFormula",
                value: "MA1");

            migrationBuilder.UpdateData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "3",
                column: "GradeCalculationFormula",
                value: "MA1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeAverage",
                table: "StudentDisciplines");

            migrationBuilder.UpdateData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "1",
                column: "GradeCalculationFormula",
                value: "MA");

            migrationBuilder.UpdateData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "2",
                column: "GradeCalculationFormula",
                value: "MA");

            migrationBuilder.UpdateData(
                table: "Disciplines",
                keyColumn: "Id",
                keyValue: "3",
                column: "GradeCalculationFormula",
                value: "MA");
        }
    }
}
