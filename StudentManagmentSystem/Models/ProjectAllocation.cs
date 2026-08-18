using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(ProjectId), nameof(StudentId), IsUnique = true)]
    [Index(nameof(StudentId))]
    [Index(nameof(FacultyId))]
    public class ProjectAllocation
    {
        [Key]
        public int AllocationId { get; set; }
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        [ForeignKey(nameof(Faculty))]
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