using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class DisciplineHomeworkAndStudentHomeworkOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Homeworks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Homeworks",
                columns: new[] { "Id", "AfterEndDateUpload", "CreationDate", "Description", "DisciplineId", "EndDate", "Grade", "Mandatory", "Penalty", "Status", "StudentId", "Title" },
                values: new object[,]
                {
                    { "1", false, new DateTime(2025, 1, 5, 18, 23, 57, 557, DateTimeKind.Local).AddTicks(8385), "Add 2+2", "1", new DateTime(2025, 1, 6, 18, 23, 57, 559, DateTimeKind.Local).AddTicks(4563), 0.0, true, 1.0, false, "df114734-138a-438a-9e96-12d02427a538", "Math1" },
                    { "2", false, new DateTime(2025, 1, 5, 18, 23, 57, 559, DateTimeKind.Local).AddTicks(5746), "F=m x _?", "2", new DateTime(2025, 1, 6, 18, 23, 57, 559, DateTimeKind.Local).AddTicks(5752), 0.0, true, 1.0, false, "10db9002-a008-4154-bdd8-9ca70870cba6", "Physics" }
                });

            migrationBuilder.InsertData(
                table: "StudentDisciplines",
                columns: new[] { "DisciplineId", "StudentId" },
                values: new object[,]
                {
                    { "1", "10db9002-a008-4154-bdd8-9ca70870cba6" },
                    { "2", "10db9002-a008-4154-bdd8-9ca70870cba6" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_StudentId",
                table: "Homeworks",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Students_StudentId",
                table: "Homeworks",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Students_StudentId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_StudentId",
                table: "Homeworks");

            migrationBuilder.DeleteData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "StudentDisciplines",
                keyColumns: new[] { "DisciplineId", "StudentId" },
                keyValues: new object[] { "1", "10db9002-a008-4154-bdd8-9ca70870cba6" });

            migrationBuilder.DeleteData(
                table: "StudentDisciplines",
                keyColumns: new[] { "DisciplineId", "StudentId" },
                keyValues: new object[] { "2", "10db9002-a008-4154-bdd8-9ca70870cba6" });

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Homeworks");
        }
    }
}
