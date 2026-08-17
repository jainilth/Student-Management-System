using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Table("Program")]
    [Index(nameof(ProgramCode), IsUnique = true)]
    [Index(nameof(DepartmentId),nameof(ProgramName),IsUnique=true)]
    public class AcademicProgram
    {
        [Key]
        public int ProgramId { get; set; }

        [Required, MaxLength(150)]
        public string ProgramName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string ProgramCode { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public int DurationYears { get; set; }
        public int TotalSemesters { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;
        public ICollection<SemesterSubject> SemesterSubjects { get; set; } = new List<SemesterSubject>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}