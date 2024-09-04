using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduLink.Migrations
{
    /// <inheritdoc />
    public partial class updateSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SessionStatus",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SessionCount",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContentAdress",
                table: "EventContents",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourseDescription",
                table: "Courses",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SessionStatus",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SessionCount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ContentAdress",
                table: "EventContents");

            migrationBuilder.DropColumn(
                name: "CourseDescription",
                table: "Courses");
        }
    }
}
