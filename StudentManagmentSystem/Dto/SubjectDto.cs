using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateSubjectDto
    {
        [Required, MaxLength(50)]
        public string SubjectCode { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string SubjectName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string SubjectType { get; set; } = string.Empty;
    }

    public class UpdateSubjectDto
    {
        [Required, MaxLength(50)]
        public string SubjectCode { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string SubjectName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string SubjectType { get; set; } = string.Empty;
    }

    public class SubjectResponseDto
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
