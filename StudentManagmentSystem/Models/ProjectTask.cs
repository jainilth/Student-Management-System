using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Table("Task")]
    [Index(nameof(ProjectAllocationId))]
    [Index(nameof(TaskStatus))]
    public class ProjectTask
    {
        [Key]
        public int TaskId { get; set; }

        public int ProjectAllocationId { get; set; }

        [Required, MaxLength(200)]
        public string TaskTitle { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string TaskDescription { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string TaskStatus { get; set; } = string.Empty;

        [Precision(5, 2)]
        public decimal AssignedScore { get; set; }

        [Precision(5, 2)]
        public decimal EarnedScore { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        [MaxLength(1000)]
        public string FacultyRemarks { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string StudentRemarks { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ProjectAllocation ProjectAllocation { get; set; } = null!;
    }
}