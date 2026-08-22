using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateSemesterResultDto
    {
        [Required]
        public int StudentSemesterId { get; set; }

        [Required]
        public decimal SGPA { get; set; }

        [Required]
        public decimal TotalCredits { get; set; }

        [Required]
        public decimal EarnedCredits { get; set; }

        [Required, MaxLength(50)]
        public string ResultStatus { get; set; } = string.Empty;
    }

    public class UpdateSemesterResultDto
    {
        [Required]
        public int StudentSemesterId { get; set; }

        [Required]
        public decimal SGPA { get; set; }

        [Required]
        public decimal TotalCredits { get; set; }

        [Required]
        public decimal EarnedCredits { get; set; }

        [Required, MaxLength(50)]
        public string ResultStatus { get; set; } = string.Empty;
    }

    public class SemesterResultResponseDto
    {
        public int SemesterResultId { get; set; }
        public int StudentSemesterId { get; set; }
        public string StudentEnrollmentNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public decimal SGPA { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal EarnedCredits { get; set; }
        public string ResultStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
