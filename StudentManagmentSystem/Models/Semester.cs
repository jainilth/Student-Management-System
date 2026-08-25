using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(SemesterNumber), IsUnique = true)]
    [Index(nameof(SemesterName), IsUnique = true)]
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }

        public int SemesterNumber { get; set; }

        [Required, MaxLength(100)]
        public string SemesterName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SemesterSubject> SemesterSubjects { get; set; } = new List<SemesterSubject>();
        public ICollection<Student> StudentsAsCurrentSemester { get; set; } = new List<Student>();
        public ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        public ICollection<FacultySubject> FacultySubjects { get; set; } = new List<FacultySubject>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}