using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(StudentSemesterId), IsUnique = true)]
    public class SemesterResult
    {
        [Key]
        public int SemesterResultId { get; set; }

        [ForeignKey(nameof(StudentSemester))]
        public int StudentSemesterId { get; set; }

        [Precision(4, 2)]
        public decimal SGPA { get; set; }

        [Precision(5, 2)]
        public decimal TotalCredits { get; set; }

        [Precision(5, 2)]
        public decimal EarnedCredits { get; set; }

        [Required, MaxLength(50)]
        public string ResultStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public StudentSemester StudentSemester { get; set; } = null!;
    }
}