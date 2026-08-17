using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(StudentId), nameof(SubjectId), nameof(SemesterId), IsUnique = true)]
    [Index(nameof(StudentId))]
    [Index(nameof(SubjectId))]
    [Index(nameof(SemesterId))]
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int SemesterId { get; set; }

        public int ClassesHeld { get; set; }
        public int ClassesAttended { get; set; }

        [Precision(5, 2)]
        public decimal AttendancePercentage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Student Student { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
        public Semester Semester { get; set; } = null!;
    }
}