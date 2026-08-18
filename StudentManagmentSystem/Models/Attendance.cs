using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(StudentSemesterId), nameof(SemesterSubjectId), IsUnique = true)]
    [Index(nameof(SemesterSubjectId))]
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        [ForeignKey(nameof(StudentSemester))]
        public int StudentSemesterId { get; set; }
        [ForeignKey(nameof(SemesterSubject))]
        public int SemesterSubjectId { get; set; }
        public int ClassesHeld { get; set; }
        public int ClassesAttended { get; set; }

        [Precision(5, 2)]
        public decimal AttendancePercentage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public StudentSemester StudentSemester { get; set; } = null!;

        public SemesterSubject SemesterSubject { get; set; } = null!;
    }
}