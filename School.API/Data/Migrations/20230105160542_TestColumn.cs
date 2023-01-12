using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Data.Migrations;

/// <inheritdoc />
public partial class TestColumn : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "Test",
            table: "Student",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Test",
            table: "Student");
    }
}
