using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedDummyStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2d266f17-ffe3-4aab-b7a7-ba275a2cc112", 0, "14a887a2-81b1-42ae-8152-d9381d231604", "harry@st.com", false, false, null, "Harry", null, null, null, null, false, "cf051c69-e7ad-4f66-96dd-b3ac3efecdfa", false, null });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "GeneralGrade", "YearOfStudy" },
                values: new object[] { "2d266f17-ffe3-4aab-b7a7-ba275a2cc112", 30.0, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: "2d266f17-ffe3-4aab-b7a7-ba275a2cc112");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2d266f17-ffe3-4aab-b7a7-ba275a2cc112");
        }
    }
}
