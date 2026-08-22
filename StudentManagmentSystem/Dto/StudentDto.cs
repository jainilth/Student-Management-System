using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateStudentDto
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string EnrollmentNumber { get; set; } = string.Empty;

        [Required]
        public int AdmissionYear { get; set; }

        [Required]
        public int ProgramId { get; set; }

        [Required]
        public int CurrentSemesterId { get; set; }
    }

    public class UpdateStudentDto
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string EnrollmentNumber { get; set; } = string.Empty;

        [Required]
        public int AdmissionYear { get; set; }

        [Required]
        public int ProgramId { get; set; }

        [Required]
        public int CurrentSemesterId { get; set; }
    }

    public class StudentResponseDto
    {
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string EnrollmentNumber { get; set; } = string.Empty;
        public int AdmissionYear { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public int CurrentSemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
