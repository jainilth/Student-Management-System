using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateFacultySubjectDto
    {
        [Required]
        public int FacultyId { get; set; }

        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public int AcademicYearId { get; set; }
    }

    public class UpdateFacultySubjectDto
    {
        [Required]
        public int FacultyId { get; set; }

        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public int AcademicYearId { get; set; }
    }

    public class FacultySubjectResponseDto
    {
        public int FacultySubjectId { get; set; }
        public int FacultyId { get; set; }
        public string FacultyEmployeeNumber { get; set; } = string.Empty;
        public string FacultyName { get; set; } = string.Empty;
        public int SemesterSubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int AcademicYearId { get; set; }
        public string AcademicYear { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
