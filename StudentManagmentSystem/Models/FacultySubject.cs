using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(FacultyId), nameof(SubjectId), nameof(SemesterId), nameof(AcademicYear), IsUnique = true)]
    [Index(nameof(FacultyId))]
    [Index(nameof(SubjectId))]
    [Index(nameof(SemesterId))]
    public class FacultySubject
    {
        [Key]
        public int FacultySubjectId { get; set; }

        public int FacultyId { get; set; }
        public int SubjectId { get; set; }
        public int SemesterId { get; set; }

        [Required, MaxLength(20)]
        public string AcademicYear { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Faculty Faculty { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
        public Semester Semester { get; set; } = null!;
    }
}