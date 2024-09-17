using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduLink.Migrations
{
    /// <inheritdoc />
    public partial class addcolumsSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionAddress",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionAddress",
                table: "Sessions");
        }
    }
}
