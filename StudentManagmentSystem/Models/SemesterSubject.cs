using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(ProgramId), nameof(SemesterId), nameof(SubjectId), IsUnique = true)]
    [Index(nameof(ProgramId), nameof(SemesterId))]
    public class SemesterSubject
    {
        [Key]
        public int SemesterSubjectId { get; set; }

        [ForeignKey(nameof(AcademicProgram))]
        public int ProgramId { get; set; }
        [ForeignKey(nameof(Semester))]
        public int SemesterId { get; set; }
        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }

        [Precision(5, 2)]
        public decimal Credits { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public AcademicProgram? AcademicProgram { get; set; } = null!;
        public Semester Semester { get; set; } = null!;
        public Subject? Subject { get; set; } = null!;

        public ICollection<SubjectResult> SubjectResults { get; set; } = new List<SubjectResult>();
        public ICollection<FacultySubject> FacultySubjects { get; set; }
        = new List<FacultySubject>();

        public ICollection<Attendance> Attendances { get; set; }
            = new List<Attendance>();

        public ICollection<ClassSession> ClassSessions { get; set; }
            = new List<ClassSession>();

        public ICollection<Material> Materials { get; set; }
            = new List<Material>();
    }
}