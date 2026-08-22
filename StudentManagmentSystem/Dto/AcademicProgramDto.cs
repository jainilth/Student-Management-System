using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateAcademicProgramDto
    {
        [Required, MaxLength(150)]
        public string ProgramName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string ProgramCode { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int DurationYears { get; set; }

        [Required]
        public int TotalSemesters { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateAcademicProgramDto
    {
        [Required, MaxLength(150)]
        public string ProgramName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string ProgramCode { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int DurationYears { get; set; }

        [Required]
        public int TotalSemesters { get; set; }

        public bool IsActive { get; set; }
    }

    public class AcademicProgramResponseDto
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string ProgramCode { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int DurationYears { get; set; }
        public int TotalSemesters { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
