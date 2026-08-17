using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(SemesterResultId), nameof(SemesterSubjectId), IsUnique = true)]
    [Index(nameof(SemesterResultId))]
    [Index(nameof(SemesterSubjectId))]
    public class SubjectResult
    {
        [Key]
        public int SubjectResultId { get; set; }

        public int SemesterResultId { get; set; }
        public int SemesterSubjectId { get; set; }

        [Precision(5, 2)]
        public decimal InternalMarks { get; set; }

        [Precision(5, 2)]
        public decimal ExternalMarks { get; set; }

        [Precision(5, 2)]
        public decimal PracticalMarks { get; set; }

        [Precision(6, 2)]
        public decimal TotalMarks { get; set; }

        [Required, MaxLength(10)]
        public string Grade { get; set; } = string.Empty;

        [Precision(4, 2)]
        public decimal GradePoint { get; set; }

        [Precision(5, 2)]
        public decimal CreditsEarned { get; set; }

        [Required, MaxLength(50)]
        public string ResultStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public SemesterResult SemesterResult { get; set; } = null!;
        public SemesterSubject SemesterSubject { get; set; } = null!;
    }
}