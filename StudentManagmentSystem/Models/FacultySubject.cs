using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(FacultyId), nameof(SemesterSubjectId), nameof(AcademicYearId), IsUnique = true)]
    [Index(nameof(SemesterSubjectId))]
    public class FacultySubject
    {
        [Key]
        public int FacultySubjectId { get; set; }

        [ForeignKey(nameof(Faculty))]
        public int FacultyId { get; set; }
        [ForeignKey(nameof(SemesterSubject))]
        public int SemesterSubjectId { get; set; }
        [ForeignKey(nameof(AcademicYear))]
        public int AcademicYearId { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Faculty? Faculty { get; set; }
        public SemesterSubject? SemesterSubject { get; set; }
        public AcademicYear? AcademicYear { get; set; }
    }
}