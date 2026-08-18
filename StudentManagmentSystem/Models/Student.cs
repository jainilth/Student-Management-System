using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(EnrollmentNumber), IsUnique = true)]
    [Index(nameof(UserId), IsUnique = true)]
    [Index(nameof(ProgramId))]
    [Index(nameof(CurrentSemesterId))]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string EnrollmentNumber { get; set; } = string.Empty;

        public int AdmissionYear { get; set; }
        [ForeignKey(nameof(AcademicProgram))]
        public int ProgramId { get; set; }
        [ForeignKey(nameof(CurrentSemester))]
        public int CurrentSemesterId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public AcademicProgram? AcademicProgram { get; set; }
        public Semester? CurrentSemester { get; set; }

        public ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();
    }
}