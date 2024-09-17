using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduLink.Migrations
{
    /// <inheritdoc />
    public partial class addBannerImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventBannerImage",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventBannerImage",
                table: "Events");
        }
    }
}
