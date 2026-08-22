using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateProjectTaskDto
    {
        [Required]
        public int ProjectAllocationId { get; set; }

        [Required, MaxLength(200)]
        public string TaskTitle { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string TaskDescription { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string TaskStatus { get; set; } = string.Empty;

        public decimal AssignedScore { get; set; }
        public decimal EarnedScore { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [MaxLength(1000)]
        public string FacultyRemarks { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string StudentRemarks { get; set; } = string.Empty;
    }

    public class UpdateProjectTaskDto
    {
        [Required]
        public int ProjectAllocationId { get; set; }

        [Required, MaxLength(200)]
        public string TaskTitle { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string TaskDescription { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string TaskStatus { get; set; } = string.Empty;

        public decimal AssignedScore { get; set; }
        public decimal EarnedScore { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [MaxLength(1000)]
        public string FacultyRemarks { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string StudentRemarks { get; set; } = string.Empty;
    }

    public class ProjectTaskResponseDto
    {
        public int TaskId { get; set; }
        public int ProjectAllocationId { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public string StudentEnrollmentNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string TaskTitle { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public string TaskStatus { get; set; } = string.Empty;
        public decimal AssignedScore { get; set; }
        public decimal EarnedScore { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string FacultyRemarks { get; set; } = string.Empty;
        public string StudentRemarks { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
