using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentSemesterUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentSemesters_StudentId_SemesterId_AcademicYearId",
                table: "StudentSemesters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudentSemesters_StudentId_SemesterId_AcademicYearId",
                table: "StudentSemesters",
                columns: new[] { "StudentId", "SemesterId", "AcademicYearId" },
                unique: true);
        }
    }
}
