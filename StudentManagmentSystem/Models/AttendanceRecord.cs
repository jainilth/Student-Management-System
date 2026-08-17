using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(StudentId), nameof(SubjectId), nameof(FacultyId), nameof(Date), IsUnique = true)]
    [Index(nameof(StudentId))]
    [Index(nameof(SubjectId))]
    [Index(nameof(FacultyId))]
    public class AttendanceRecord
    {
        [Key]
        public int AttendanceRecordId { get; set; }

        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int FacultyId { get; set; }

        public DateTime Date { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Remarks { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Student Student { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
        public Faculty Faculty { get; set; } = null!;
    }
}