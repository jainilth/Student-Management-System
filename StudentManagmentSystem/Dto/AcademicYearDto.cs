using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateAcademicYearDto
    {
        [Required, MaxLength(20)]
        public string Year { get; set; } = string.Empty;
    }

    public class UpdateAcademicYearDto
    {
        [Required, MaxLength(20)]
        public string Year { get; set; } = string.Empty;
    }

    public class AcademicYearResponseDto
    {
        public int AcademicYearId { get; set; }
        public string Year { get; set; } = string.Empty;
    }
}
