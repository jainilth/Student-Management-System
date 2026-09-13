using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddGradePointsToSubjectResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SubjectResult_CreditsEarned",
                table: "SubjectResults");

            migrationBuilder.RenameColumn(
                name: "CreditsEarned",
                table: "SubjectResults",
                newName: "GradePoints");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SubjectResult_GradePoints",
                table: "SubjectResults",
                sql: "[GradePoints] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SubjectResult_GradePoints",
                table: "SubjectResults");

            migrationBuilder.RenameColumn(
                name: "GradePoints",
                table: "SubjectResults",
                newName: "CreditsEarned");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SubjectResult_CreditsEarned",
                table: "SubjectResults",
                sql: "[CreditsEarned] >= 0");
        }
    }
}
