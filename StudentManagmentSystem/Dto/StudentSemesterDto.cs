using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateStudentSemesterDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        [Required]
        public int AcademicYearId { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateStudentSemesterDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        [Required]
        public int AcademicYearId { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }

    public class StudentSemesterResponseDto
    {
        public int StudentSemesterId { get; set; }
        public int StudentId { get; set; }
        public string StudentEnrollmentNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int AcademicProgramId { get; set; }
        public string AcademicProgramName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int AcademicYearId { get; set; }
        public string AcademicYear { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
