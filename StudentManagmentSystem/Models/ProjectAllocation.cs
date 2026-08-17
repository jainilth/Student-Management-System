using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(ProjectId), nameof(StudentId), nameof(FacultyId), IsUnique = true)]
    [Index(nameof(ProjectId))]
    [Index(nameof(StudentId))]
    [Index(nameof(FacultyId))]
    public class ProjectAllocation
    {
        [Key]
        public int AllocationId { get; set; }

        public int ProjectId { get; set; }
        public int StudentId { get; set; }
        public int FacultyId { get; set; }

        [Precision(5, 2)]
        public decimal FinalScore { get; set; }

        [Required, MaxLength(10)]
        public string Grade { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500)]
        public string RepositoryUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Project Project { get; set; } = null!;
        public Student Student { get; set; } = null!;
        public Faculty Faculty { get; set; } = null!;

        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}