using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicYear",
                columns: table => new
                {
                    AcademicYearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYear", x => x.AcademicYearId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    GradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradePoint = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    SemesterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterNumber = table.Column<int>(type: "int", nullable: false),
                    SemesterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.SemesterId);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SubjectType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Program",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProgramCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DurationYears = table.Column<int>(type: "int", nullable: false),
                    TotalSemesters = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program", x => x.ProgramId);
                    table.CheckConstraint("CK_Program_DurationYears", "[DurationYears] > 0");
                    table.CheckConstraint("CK_Program_TotalSemesters", "[TotalSemesters] > 0");
                    table.ForeignKey(
                        name: "FK_Program_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Mobilenumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActivate = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                    table.CheckConstraint("CK_Project_DateRange", "[EndDate] >= [StartDate]");
                    table.ForeignKey(
                        name: "FK_Project_Program_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Program",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SemesterSubjects",
                columns: table => new
                {
                    SemesterSubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    Credits = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterSubjects", x => x.SemesterSubjectId);
                    table.CheckConstraint("CK_SemesterSubject_Credits", "[Credits] > 0");
                    table.ForeignKey(
                        name: "FK_SemesterSubjects_Program_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Program",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SemesterSubjects_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SemesterSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyId);
                    table.ForeignKey(
                        name: "FK_Faculties_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Faculties_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdmissionYear = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    CurrentSemesterId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Program_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Program",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Semesters_CurrentSemesterId",
                        column: x => x.CurrentSemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId");
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SemesterSubjectId = table.Column<int>(type: "int", nullable: false),
                    UploadedBy = table.Column<int>(type: "int", nullable: false),
                    MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialId);
                    table.CheckConstraint("CK_Material_FileSize", "[FileSize] > 0");
                    table.ForeignKey(
                        name: "FK_Materials_SemesterSubjects_SemesterSubjectId",
                        column: x => x.SemesterSubjectId,
                        principalTable: "SemesterSubjects",
                        principalColumn: "SemesterSubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materials_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId");
                    table.ForeignKey(
                        name: "FK_Materials_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                    table.ForeignKey(
                        name: "FK_Materials_Users_UploadedBy",
                        column: x => x.UploadedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassSession",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterSubjectId = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    SessionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSession", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_ClassSession_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSession_SemesterSubjects_SemesterSubjectId",
                        column: x => x.SemesterSubjectId,
                        principalTable: "SemesterSubjects",
                        principalColumn: "SemesterSubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacultySubjects",
                columns: table => new
                {
                    FacultySubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    SemesterSubjectId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacultySubjects", x => x.FacultySubjectId);
                    table.ForeignKey(
                        name: "FK_FacultySubjects_AcademicYear_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYear",
                        principalColumn: "AcademicYearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacultySubjects_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacultySubjects_SemesterSubjects_SemesterSubjectId",
                        column: x => x.SemesterSubjectId,
                        principalTable: "SemesterSubjects",
                        principalColumn: "SemesterSubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacultySubjects_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId");
                    table.ForeignKey(
                        name: "FK_FacultySubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateTable(
                name: "ProjectAllocations",
                columns: table => new
                {
                    AllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    FinalScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RepositoryUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAllocations", x => x.AllocationId);
                    table.CheckConstraint("CK_ProjectAllocation_FinalScore", "[FinalScore] IS NULL OR ([FinalScore] >= 0 AND [FinalScore] <= 100)");
                    table.ForeignKey(
                        name: "FK_ProjectAllocations_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectAllocations_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectAllocations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentSemesters",
                columns: table => new
                {
                    StudentSemesterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSemesters", x => x.StudentSemesterId);
                    table.ForeignKey(
                        name: "FK_StudentSemesters_AcademicYear_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYear",
                        principalColumn: "AcademicYearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentSemesters_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentSemesters_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectAllocationId = table.Column<int>(type: "int", nullable: false),
                    TaskTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TaskStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    EarnedScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FacultyRemarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StudentRemarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                    table.CheckConstraint("CK_ProjectTask_CompletedDate", "[CompletedDate] IS NULL OR [CompletedDate] >= [StartDate]");
                    table.CheckConstraint("CK_ProjectTask_DateRange", "[DueDate] >= [StartDate]");
                    table.CheckConstraint("CK_ProjectTask_Scores", "[AssignedScore] >= 0 AND [EarnedScore] >= 0 AND [EarnedScore] <= [AssignedScore]");
                    table.ForeignKey(
                        name: "FK_Task_ProjectAllocations_ProjectAllocationId",
                        column: x => x.ProjectAllocationId,
                        principalTable: "ProjectAllocations",
                        principalColumn: "AllocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    AttendanceRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    StudentSemesterId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.AttendanceRecordId);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_ClassSession_SessionId",
                        column: x => x.SessionId,
                        principalTable: "ClassSession",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId");
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_StudentSemesters_StudentSemesterId",
                        column: x => x.StudentSemesterId,
                        principalTable: "StudentSemesters",
                        principalColumn: "StudentSemesterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentSemesterId = table.Column<int>(type: "int", nullable: false),
                    SemesterSubjectId = table.Column<int>(type: "int", nullable: false),
                    ClassesHeld = table.Column<int>(type: "int", nullable: false),
                    ClassesAttended = table.Column<int>(type: "int", nullable: false),
                    AttendancePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                    table.CheckConstraint("CK_Attendance_Classes", "[ClassesHeld] >= 0 AND [ClassesAttended] >= 0 AND [ClassesAttended] <= [ClassesHeld]");
                    table.CheckConstraint("CK_Attendance_Percentage", "[AttendancePercentage] >= 0 AND [AttendancePercentage] <= 100");
                    table.ForeignKey(
                        name: "FK_Attendances_SemesterSubjects_SemesterSubjectId",
                        column: x => x.SemesterSubjectId,
                        principalTable: "SemesterSubjects",
                        principalColumn: "SemesterSubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId");
                    table.ForeignKey(
                        name: "FK_Attendances_StudentSemesters_StudentSemesterId",
                        column: x => x.StudentSemesterId,
                        principalTable: "StudentSemesters",
                        principalColumn: "StudentSemesterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                    table.ForeignKey(
                        name: "FK_Attendances_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateTable(
                name: "SemesterResults",
                columns: table => new
                {
                    SemesterResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentSemesterId = table.Column<int>(type: "int", nullable: false),
                    SGPA = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    TotalCredits = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    EarnedCredits = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ResultStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterResults", x => x.SemesterResultId);
                    table.CheckConstraint("CK_SemesterResult_Credits", "[EarnedCredits] >= 0 AND [EarnedCredits] <= [TotalCredits]");
                    table.CheckConstraint("CK_SemesterResult_SGPA", "[SGPA] >= 0 AND [SGPA] <= 10");
                    table.ForeignKey(
                        name: "FK_SemesterResults_StudentSemesters_StudentSemesterId",
                        column: x => x.StudentSemesterId,
                        principalTable: "StudentSemesters",
                        principalColumn: "StudentSemesterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectResults",
                columns: table => new
                {
                    SubjectResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterResultId = table.Column<int>(type: "int", nullable: false),
                    SemesterSubjectId = table.Column<int>(type: "int", nullable: false),
                    InternalMarks = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ExternalMarks = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    PracticalMarks = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TotalMarks = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: false),
                    CreditsEarned = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ResultStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectResults", x => x.SubjectResultId);
                    table.CheckConstraint("CK_SubjectResult_CreditsEarned", "[CreditsEarned] >= 0");
                    table.CheckConstraint("CK_SubjectResult_Marks", "[InternalMarks] >= 0 AND [ExternalMarks] >= 0 AND [PracticalMarks] >= 0 AND [TotalMarks] >= 0");
                    table.ForeignKey(
                        name: "FK_SubjectResults_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectResults_SemesterResults_SemesterResultId",
                        column: x => x.SemesterResultId,
                        principalTable: "SemesterResults",
                        principalColumn: "SemesterResultId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectResults_SemesterSubjects_SemesterSubjectId",
                        column: x => x.SemesterSubjectId,
                        principalTable: "SemesterSubjects",
                        principalColumn: "SemesterSubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_FacultyId",
                table: "AttendanceRecords",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_SessionId_StudentSemesterId",
                table: "AttendanceRecords",
                columns: new[] { "SessionId", "StudentSemesterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_StudentId",
                table: "AttendanceRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_StudentSemesterId",
                table: "AttendanceRecords",
                column: "StudentSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_SubjectId",
                table: "AttendanceRecords",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SemesterId",
                table: "Attendances",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SemesterSubjectId",
                table: "Attendances",
                column: "SemesterSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentSemesterId_SemesterSubjectId",
                table: "Attendances",
                columns: new[] { "StudentSemesterId", "SemesterSubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SubjectId",
                table: "Attendances",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSession_FacultyId_SessionDate",
                table: "ClassSession",
                columns: new[] { "FacultyId", "SessionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSession_SemesterSubjectId_SessionDate",
                table: "ClassSession",
                columns: new[] { "SemesterSubjectId", "SessionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentCode",
                table: "Departments",
                column: "DepartmentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentName",
                table: "Departments",
                column: "DepartmentName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_DepartmentId",
                table: "Faculties",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_EmployeeNumber",
                table: "Faculties",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_UserId",
                table: "Faculties",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacultySubjects_AcademicYearId",
                table: "FacultySubjects",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_FacultySubjects_FacultyId_SemesterSubjectId_AcademicYearId",
                table: "FacultySubjects",
                columns: new[] { "FacultyId", "SemesterSubjectId", "AcademicYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacultySubjects_SemesterId",
                table: "FacultySubjects",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_FacultySubjects_SemesterSubjectId",
                table: "FacultySubjects",
                column: "SemesterSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FacultySubjects_SubjectId",
                table: "FacultySubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SemesterId",
                table: "Materials",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SemesterSubjectId",
                table: "Materials",
                column: "SemesterSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SubjectId",
                table: "Materials",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UploadedBy",
                table: "Materials",
                column: "UploadedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Program_DepartmentId_ProgramName",
                table: "Program",
                columns: new[] { "DepartmentId", "ProgramName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Program_ProgramCode",
                table: "Program",
                column: "ProgramCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProgramId",
                table: "Project",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_SemesterId_ProgramId",
                table: "Project",
                columns: new[] { "SemesterId", "ProgramId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAllocations_FacultyId",
                table: "ProjectAllocations",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAllocations_ProjectId_StudentId",
                table: "ProjectAllocations",
                columns: new[] { "ProjectId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAllocations_StudentId",
                table: "ProjectAllocations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemesterResults_StudentSemesterId",
                table: "SemesterResults",
                column: "StudentSemesterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_SemesterName",
                table: "Semesters",
                column: "SemesterName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_SemesterNumber",
                table: "Semesters",
                column: "SemesterNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemesterSubjects_ProgramId_SemesterId",
                table: "SemesterSubjects",
                columns: new[] { "ProgramId", "SemesterId" });

            migrationBuilder.CreateIndex(
                name: "IX_SemesterSubjects_ProgramId_SemesterId_SubjectId",
                table: "SemesterSubjects",
                columns: new[] { "ProgramId", "SemesterId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemesterSubjects_SemesterId",
                table: "SemesterSubjects",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterSubjects_SubjectId",
                table: "SemesterSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CurrentSemesterId",
                table: "Students",
                column: "CurrentSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_EnrollmentNumber",
                table: "Students",
                column: "EnrollmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProgramId",
                table: "Students",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SemesterId",
                table: "Students",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentSemesters_AcademicYearId",
                table: "StudentSemesters",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSemesters_SemesterId",
                table: "StudentSemesters",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSemesters_StudentId_SemesterId_AcademicYearId",
                table: "StudentSemesters",
                columns: new[] { "StudentId", "SemesterId", "AcademicYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectResults_GradeId",
                table: "SubjectResults",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectResults_SemesterResultId",
                table: "SubjectResults",
                column: "SemesterResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectResults_SemesterResultId_SemesterSubjectId",
                table: "SubjectResults",
                columns: new[] { "SemesterResultId", "SemesterSubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectResults_SemesterSubjectId",
                table: "SubjectResults",
                column: "SemesterSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectCode",
                table: "Subjects",
                column: "SubjectCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_DueDate",
                table: "Task",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ProjectAllocationId_TaskStatus",
                table: "Task",
                columns: new[] { "ProjectAllocationId", "TaskStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "FacultySubjects");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "SubjectResults");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "ClassSession");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "SemesterResults");

            migrationBuilder.DropTable(
                name: "ProjectAllocations");

            migrationBuilder.DropTable(
                name: "SemesterSubjects");

            migrationBuilder.DropTable(
                name: "StudentSemesters");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "AcademicYear");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Program");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
