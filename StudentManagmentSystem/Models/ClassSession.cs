using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(SemesterSubjectId), nameof(SessionDate))]
    [Index(nameof(FacultyId), nameof(SessionDate))]
    public class ClassSession
    {
        [Key]
        public int SessionId { get; set; }
        [ForeignKey(nameof(SemesterSubject))]
        public int SemesterSubjectId { get; set; }
        [ForeignKey(nameof(Faculty))]
        public int FacultyId { get; set; }

        public DateTime SessionDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string? Topic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public SemesterSubject SemesterSubject { get; set; } = null!;

        public Faculty Faculty { get; set; } = null!;

        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
            = new List<AttendanceRecord>();
    }
}
