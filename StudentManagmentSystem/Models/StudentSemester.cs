using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(StudentId), nameof(SemesterId), IsUnique = true)]
    [Index(nameof(StudentId))]
    [Index(nameof(SemesterId))]
    public class StudentSemester
    {
        [Key]
        public int StudentSemesterId { get; set; }

        public int StudentId { get; set; }
        public int SemesterId { get; set; }

        [Required, MaxLength(20)]
        public string AcademicYear { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Student Student { get; set; } = null!;
        public Semester Semester { get; set; } = null!;

        public SemesterResult? SemesterResult { get; set; }
    }
}