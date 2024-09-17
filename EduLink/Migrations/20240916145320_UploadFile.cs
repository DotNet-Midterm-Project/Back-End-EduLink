using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduLink.Migrations
{
    /// <inheritdoc />
    public partial class UploadFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArticleFile",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleFile",
                table: "Articles");
        }
    }
}
