using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ProjectHomeworkWithFiles : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Homeworks",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProjectFile",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeworkID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFile_Homeworks_HomeworkID",
                        column: x => x.HomeworkID,
                        principalTable: "Homeworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Homeworks",
                columns: new[] { "Id", "AfterEndDateUpload", "Content", "CreationDate", "Description", "DisciplineId", "Discriminator", "EndDate", "Grade", "IsTemplate", "Mandatory", "Penalty", "Status", "StudentId", "Title" },
                values: new object[,]
                {
                    { "1", false, "", null, "Add 2+2", "1", "Homework", null, 0.0, null, true, 1.0, false, "df114734-138a-438a-9e96-12d02427a538", "Math1" },
                    { "2", false, "", null, "F=m x _?", "2", "Homework", null, 0.0, null, true, 1.0, false, "10db9002-a008-4154-bdd8-9ca70870cba6", "Physics" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFile_HomeworkID",
                table: "ProjectFile",
                column: "HomeworkID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFile");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Homeworks");
        }
    }
}
