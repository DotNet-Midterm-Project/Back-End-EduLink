using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduLink.Migrations
{
    /// <inheritdoc />
    public partial class addrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentCourses_Courses_CourseID",
                table: "DepartmentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentCourses_Departments_DepartmentID",
                table: "DepartmentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerCourses_Courses_CourseID",
                table: "VolunteerCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerCourses_Volunteers_VolunteerID",
                table: "VolunteerCourses");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentCourses_Courses_CourseID",
                table: "DepartmentCourses",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentCourses_Departments_DepartmentID",
                table: "DepartmentCourses",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerCourses_Courses_CourseID",
                table: "VolunteerCourses",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerCourses_Volunteers_VolunteerID",
                table: "VolunteerCourses",
                column: "VolunteerID",
                principalTable: "Volunteers",
                principalColumn: "VolunteerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentCourses_Courses_CourseID",
                table: "DepartmentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentCourses_Departments_DepartmentID",
                table: "DepartmentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerCourses_Courses_CourseID",
                table: "VolunteerCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerCourses_Volunteers_VolunteerID",
                table: "VolunteerCourses");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentCourses_Courses_CourseID",
                table: "DepartmentCourses",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentCourses_Departments_DepartmentID",
                table: "DepartmentCourses",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerCourses_Courses_CourseID",
                table: "VolunteerCourses",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerCourses_Volunteers_VolunteerID",
                table: "VolunteerCourses",
                column: "VolunteerID",
                principalTable: "Volunteers",
                principalColumn: "VolunteerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
