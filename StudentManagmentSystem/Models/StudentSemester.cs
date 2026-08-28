using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(StudentId), nameof(SemesterId), nameof(AcademicYearId), IsUnique = true)]
    [Index(nameof(AcademicYearId))]
    [Index(nameof(SemesterId))]
    public class StudentSemester
    {
        [Key]
        public int StudentSemesterId { get; set; }

        public int StudentId { get; set; }
        public int SemesterId { get; set; }
        public int AcademicYearId { get; set; }


        public DateTime EnrollmentDate { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Student? Student { get; set; } = null!;
        public AcademicYear? AcademicYear { get; set; }
        public Semester? Semester { get; set; } = null!;

        public SemesterResult? SemesterResult { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();

        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
            = new List<AttendanceRecord>();

        public ICollection<SubjectResult> SubjectResults { get; set; } = new List<SubjectResult>();
    }
}