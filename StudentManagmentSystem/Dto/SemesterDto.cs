using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateSemesterDto
    {
        public int SemesterNumber { get; set; }

        [Required, MaxLength(100)]
        public string SemesterName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateSemesterDto
    {
        public int SemesterNumber { get; set; }

        [Required, MaxLength(100)]
        public string SemesterName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }

    public class SemesterResponseDto
    {
        public int SemesterId { get; set; }
        public int SemesterNumber { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
