using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateSubjectResultDto
    {
        [Required]
        public int SemesterResultId { get; set; }

        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public decimal InternalMarks { get; set; }

        [Required]
        public decimal ExternalMarks { get; set; }

        [Required]
        public decimal PracticalMarks { get; set; }

        [Required]
        public decimal TotalMarks { get; set; }

        [Required]
        public int GradeId { get; set; }

        [Required]
        public decimal CreditsEarned { get; set; }

        [Required, MaxLength(50)]
        public string ResultStatus { get; set; } = string.Empty;
    }

    public class UpdateSubjectResultDto
    {
        [Required]
        public int SemesterResultId { get; set; }

        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public decimal InternalMarks { get; set; }

        [Required]
        public decimal ExternalMarks { get; set; }

        [Required]
        public decimal PracticalMarks { get; set; }

        [Required]
        public decimal TotalMarks { get; set; }

        [Required]
        public int GradeId { get; set; }

        [Required]
        public decimal CreditsEarned { get; set; }

        [Required, MaxLength(50)]
        public string ResultStatus { get; set; } = string.Empty;
    }

    public class SubjectResultResponseDto
    {
        public int SubjectResultId { get; set; }
        public int SemesterResultId { get; set; }
        public string StudentEnrollmentNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int SemesterSubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public decimal InternalMarks { get; set; }
        public decimal ExternalMarks { get; set; }
        public decimal PracticalMarks { get; set; }
        public decimal TotalMarks { get; set; }
        public int GradeId { get; set; }
        public string GradeCode { get; set; } = string.Empty;
        public decimal CreditsEarned { get; set; }
        public string ResultStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
