using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class DisciplineHomeworkOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Homeworks",
                columns: new[] { "Id", "AfterEndDateUpload", "CreationDate", "Description", "DisciplineId", "EndDate", "Grade", "Mandatory", "Penalty", "Status", "StudentId", "Title" },
                values: new object[,]
                {
                    { "1", false, new DateTime(2025, 1, 5, 18, 23, 57, 557, DateTimeKind.Local).AddTicks(8385), "Add 2+2", "1", new DateTime(2025, 1, 6, 18, 23, 57, 559, DateTimeKind.Local).AddTicks(4563), 0.0, true, 1.0, false, "df114734-138a-438a-9e96-12d02427a538", "Math1" },
                    { "2", false, new DateTime(2025, 1, 5, 18, 23, 57, 559, DateTimeKind.Local).AddTicks(5746), "F=m x _?", "2", new DateTime(2025, 1, 6, 18, 23, 57, 559, DateTimeKind.Local).AddTicks(5752), 0.0, true, 1.0, false, "10db9002-a008-4154-bdd8-9ca70870cba6", "Physics" }
                });
        }
    }
}
