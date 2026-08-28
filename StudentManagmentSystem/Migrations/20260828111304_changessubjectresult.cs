using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class changessubjectresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectResults_SemesterResults_SemesterResultId",
                table: "SubjectResults");

            migrationBuilder.RenameColumn(
                name: "SemesterResultId",
                table: "SubjectResults",
                newName: "StudentSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectResults_SemesterResultId_SemesterSubjectId",
                table: "SubjectResults",
                newName: "IX_SubjectResults_StudentSemesterId_SemesterSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectResults_SemesterResultId",
                table: "SubjectResults",
                newName: "IX_SubjectResults_StudentSemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectResults_StudentSemesters_StudentSemesterId",
                table: "SubjectResults",
                column: "StudentSemesterId",
                principalTable: "StudentSemesters",
                principalColumn: "StudentSemesterId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectResults_StudentSemesters_StudentSemesterId",
                table: "SubjectResults");

            migrationBuilder.RenameColumn(
                name: "StudentSemesterId",
                table: "SubjectResults",
                newName: "SemesterResultId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectResults_StudentSemesterId_SemesterSubjectId",
                table: "SubjectResults",
                newName: "IX_SubjectResults_SemesterResultId_SemesterSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectResults_StudentSemesterId",
                table: "SubjectResults",
                newName: "IX_SubjectResults_SemesterResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectResults_SemesterResults_SemesterResultId",
                table: "SubjectResults",
                column: "SemesterResultId",
                principalTable: "SemesterResults",
                principalColumn: "SemesterResultId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
