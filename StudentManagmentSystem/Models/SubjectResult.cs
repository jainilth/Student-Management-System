using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(StudentSemesterId), nameof(SemesterSubjectId), IsUnique = true)]
    [Index(nameof(StudentSemesterId))]
    [Index(nameof(SemesterSubjectId))]
    public class SubjectResult
    {
        [Key]
        public int SubjectResultId { get; set; }
        [ForeignKey(nameof(StudentSemester))]
        public int StudentSemesterId { get; set; }
        [ForeignKey(nameof(SemesterSubject))]
        public int SemesterSubjectId { get; set; }

        [Precision(5, 2)]
        public decimal InternalMarks { get; set; }

        [Precision(5, 2)]
        public decimal ExternalMarks { get; set; }

        [Precision(5, 2)]
        public decimal PracticalMarks { get; set; }

        [Precision(6, 2)]
        public decimal TotalMarks { get; set; }
        [ForeignKey(nameof(Grade))]
        public int GradeId { get; set; }

        [Precision(5, 2)]
        public decimal CreditsEarned { get; set; }

        [Required, MaxLength(50)]
        public string ResultStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Grade Grade { get; set; } = null!;
        public StudentSemester StudentSemester { get; set; } = null!;
        public SemesterSubject SemesterSubject { get; set; } = null!;
    }
}