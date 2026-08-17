using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(ProgramId), nameof(SemesterId), nameof(SubjectId), IsUnique = true)]
    [Index(nameof(ProgramId))]
    [Index(nameof(SemesterId))]
    [Index(nameof(SubjectId))]
    public class SemesterSubject
    {
        [Key]
        public int SemesterSubjectId { get; set; }

        public int ProgramId { get; set; }
        public int SemesterId { get; set; }
        public int SubjectId { get; set; }

        [Precision(5, 2)]
        public decimal Credits { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public AcademicProgram AcademicProgram { get; set; } = null!;
        public Semester Semester { get; set; } = null!;
        public Subject Subject { get; set; } = null!;

        public ICollection<SubjectResult> SubjectResults { get; set; } = new List<SubjectResult>();
    }
}