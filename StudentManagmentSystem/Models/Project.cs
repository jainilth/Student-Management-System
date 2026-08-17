using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Table("Project")]
    [Index(nameof(SemesterId))]
    [Index(nameof(ProgramId))]
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public int SemesterId { get; set; }
        public int ProgramId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Semester Semester { get; set; } = null!;
        public AcademicProgram AcademicProgram { get; set; } = null!;

        public ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();
    }
}