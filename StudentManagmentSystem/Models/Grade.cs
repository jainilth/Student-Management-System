using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }

        public string GradeCode { get; set; } = null!;
        public string GradeName { get; set; } = null!;
        public decimal GradePoint { get; set; }

        public decimal MinMarks { get; set; }
        public decimal MaxMarks { get; set; }

        // Navigation property
        public ICollection<SubjectResult> SubjectResults { get; set; }
            = new List<SubjectResult>();
    }
}
