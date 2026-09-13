using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalMarksFromSubjectResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SubjectResult_Marks",
                table: "SubjectResults");

            migrationBuilder.DropColumn(
                name: "TotalMarks",
                table: "SubjectResults");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SubjectResult_Marks",
                table: "SubjectResults",
                sql: "[InternalMarks] >= 0 AND [ExternalMarks] >= 0 AND [PracticalMarks] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SubjectResult_Marks",
                table: "SubjectResults");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalMarks",
                table: "SubjectResults",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "CK_SubjectResult_Marks",
                table: "SubjectResults",
                sql: "[InternalMarks] >= 0 AND [ExternalMarks] >= 0 AND [PracticalMarks] >= 0 AND [TotalMarks] >= 0");
        }
    }
}
