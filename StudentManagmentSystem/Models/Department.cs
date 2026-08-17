using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(DepartmentCode), IsUnique = true)]
    [Index(nameof(DepartmentName), IsUnique = true)]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required, MaxLength(150)]
        public string DepartmentName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string DepartmentCode { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AcademicProgram> Programs { get; set; } = new List<AcademicProgram>();
        public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
    }
}