using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateClassSessionDto
    {
        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public DateTime SessionDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [MaxLength(500)]
        public string? Topic { get; set; }
    }

    public class UpdateClassSessionDto
    {
        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public DateTime SessionDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [MaxLength(500)]
        public string? Topic { get; set; }
    }

    public class ClassSessionResponseDto
    {
        public int SessionId { get; set; }
        public int SemesterSubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int FacultyId { get; set; }
        public string FacultyName { get; set; } = string.Empty;
        public DateTime SessionDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Topic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
