using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateSemesterSubjectDto
    {
        [Required]
        public int ProgramId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public decimal Credits { get; set; }
    }

    public class UpdateSemesterSubjectDto
    {
        [Required]
        public int ProgramId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public decimal Credits { get; set; }
    }

    public class SemesterSubjectResponseDto
    {
        public int SemesterSubjectId { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public decimal Credits { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
