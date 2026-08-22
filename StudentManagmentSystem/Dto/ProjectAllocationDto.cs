using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateProjectAllocationDto
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        public decimal FinalScore { get; set; }

        [Required, MaxLength(10)]
        public string Grade { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500)]
        public string RepositoryUrl { get; set; } = string.Empty;
    }

    public class UpdateProjectAllocationDto
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        public decimal FinalScore { get; set; }

        [Required, MaxLength(10)]
        public string Grade { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500)]
        public string RepositoryUrl { get; set; } = string.Empty;
    }

    public class ProjectAllocationResponseDto
    {
        public int AllocationId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentEnrollmentNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int FacultyId { get; set; }
        public string FacultyEmployeeNumber { get; set; } = string.Empty;
        public string FacultyName { get; set; } = string.Empty;
        public decimal? FinalScore { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RepositoryUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
