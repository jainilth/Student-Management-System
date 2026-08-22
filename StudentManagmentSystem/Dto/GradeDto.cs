using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateGradeDto
    {
        [Required]
        public string GradeCode { get; set; } = string.Empty;

        [Required]
        public string GradeName { get; set; } = string.Empty;

        public decimal GradePoint { get; set; }
        public decimal MinMarks { get; set; }
        public decimal MaxMarks { get; set; }
    }

    public class UpdateGradeDto
    {
        [Required]
        public string GradeCode { get; set; } = string.Empty;

        [Required]
        public string GradeName { get; set; } = string.Empty;

        public decimal GradePoint { get; set; }
        public decimal MinMarks { get; set; }
        public decimal MaxMarks { get; set; }
    }

    public class GradeResponseDto
    {
        public int GradeId { get; set; }
        public string GradeCode { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
        public decimal GradePoint { get; set; }
        public decimal MinMarks { get; set; }
        public decimal MaxMarks { get; set; }
    }
}
