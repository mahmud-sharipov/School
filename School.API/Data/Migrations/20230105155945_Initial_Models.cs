using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Data.Migrations;

/// <inheritdoc />
public partial class InitialModels : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Subject",
            columns: table => new
            {
                Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Subject", x => x.Guid);
            });

        migrationBuilder.CreateTable(
            name: "Teacher",
            columns: table => new
            {
                Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                WorkExperience = table.Column<int>(type: "int", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Gender = table.Column<int>(type: "int", nullable: false),
                Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Teacher", x => x.Guid);
            });

        migrationBuilder.CreateTable(
            name: "Group",
            columns: table => new
            {
                Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Grade = table.Column<int>(type: "int", nullable: false),
                TeacherGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Group", x => x.Guid);
                table.ForeignKey(
                    name: "FK_Group_Teacher_TeacherGuid",
                    column: x => x.TeacherGuid,
                    principalTable: "Teacher",
                    principalColumn: "Guid",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Student",
            columns: table => new
            {
                Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GroupGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Gender = table.Column<int>(type: "int", nullable: false),
                Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Student", x => x.Guid);
                table.ForeignKey(
                    name: "FK_Student_Group_GroupGuid",
                    column: x => x.GroupGuid,
                    principalTable: "Group",
                    principalColumn: "Guid",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Group_TeacherGuid",
            table: "Group",
            column: "TeacherGuid");

        migrationBuilder.CreateIndex(
            name: "IX_Student_GroupGuid",
            table: "Student",
            column: "GroupGuid");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Student");

        migrationBuilder.DropTable(
            name: "Subject");

        migrationBuilder.DropTable(
            name: "Group");

        migrationBuilder.DropTable(
            name: "Teacher");
    }
}
