using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(EmployeeNumber), IsUnique = true)]
    [Index(nameof(UserId), IsUnique = true)]
    [Index(nameof(DepartmentId))]
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string EmployeeNumber { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Department Department { get; set; } = null!;

        public ICollection<FacultySubject> FacultySubjects { get; set; } = new List<FacultySubject>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();
    }
}