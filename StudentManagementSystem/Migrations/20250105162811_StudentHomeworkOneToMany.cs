using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class StudentHomeworkOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Homeworks",
                columns: new[] { "Id", "AfterEndDateUpload", "CreationDate", "Description", "DisciplineId", "EndDate", "Grade", "Mandatory", "Penalty", "Status", "StudentId", "Title" },
                values: new object[,]
                {
                    { "1", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Add 2+2", "1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, true, 1.0, false, "df114734-138a-438a-9e96-12d02427a538", "Math1" },
                    { "2", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "F=m x _?", "2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, true, 1.0, false, "10db9002-a008-4154-bdd8-9ca70870cba6", "Physics" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
