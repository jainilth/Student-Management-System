using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(SubjectCode), IsUnique = true)]
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required, MaxLength(50)]
        public string SubjectCode { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string SubjectName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string SubjectType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SemesterSubject> SemesterSubjects { get; set; } = new List<SemesterSubject>();
        public ICollection<FacultySubject> FacultySubjects { get; set; } = new List<FacultySubject>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}